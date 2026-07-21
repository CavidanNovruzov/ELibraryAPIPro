using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Abstractions.Services.Auth;
using ELibraryAPI.Application.Dtos.Auth;
using ELibraryAPI.Application.Options;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Domain.Enums;
using ELibraryAPI.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace ELibraryAPI.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IEmailSender _emailSender;
    private readonly SmtpOptions _emailOptions;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtGenerator;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly JwtOptions _jwtOptions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IJwtTokenGenerator jwtGenerator,
        IRefreshTokenService refreshTokenService,
        IOptions<JwtOptions> jwtOptions,
        IEmailSender emailSender,
        IOptions<SmtpOptions> emailOptions,
        IUnitOfWork unitOfWork,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtGenerator = jwtGenerator;
        _refreshTokenService = refreshTokenService;
        _jwtOptions = jwtOptions.Value;
        _emailSender = emailSender;
        _emailOptions = emailOptions.Value;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> RegisterAsync(RegistrRequest request, CancellationToken ct = default)
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName.Trim(),
            Email = request.Email.Trim(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return Result<Guid>.Failure(string.Join(" ", result.Errors.Select(e => e.Description)), ErrorType.ValidationError);

        await _userManager.AddToRoleAsync(user, RoleNames.User);

        await _unitOfWork.WriteRepository<Basket, Guid>().AddAsync(new Basket { UserId = user.Id }, ct);
        await _unitOfWork.WriteRepository<Wishlist, Guid>().AddAsync(new Wishlist { UserId = user.Id }, ct);

        if (_emailOptions.SendEmails)
        {
            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"{_emailOptions.ConfirmEmailBaseUrl}?userId={user.Id}&token={WebUtility.UrlEncode(token)}";

                await _emailSender.SendEmailAsync(
                    user.Email!,
                    "Hesabın Təsdiqlənməsi — ELibrary.az",
                    $"Zəhmət olmasa, hesabınızı təsdiqləmək üçün <a href='{confirmationLink}'>bura klikləyin</a>.",
                    null,
                    ct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Confirmation email could not be sent for user {UserId}.", user.Id);
            }
        }

        return Result<Guid>.Success(user.Id, "Qeydiyyat uğurla tamamlandı. Zəhmət olmasa, hesabınızı təsdiqləmək üçün email ünvanınızı yoxlayın.");
    }

    public async Task<Result<TokenResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var loginInput = request.Login.Trim();

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.UserName == loginInput || u.Email == loginInput, ct);

        if (user == null)
            return Result<TokenResponse>.Failure("İstifadəçi adı və ya şifrə yanlışdır.", ErrorType.Unauthorized);

        if (!user.IsActive)
            return Result<TokenResponse>.Failure("Sizin hesabınız deaktiv edilib. Dəstək ilə əlaqə saxlayın.", ErrorType.Forbidden);

        if (!user.EmailConfirmed)
            return Result<TokenResponse>.Failure("Zəhmət olmasa, əvvəlcə email ünvanınızı təsdiqləyin.", ErrorType.Forbidden);

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                return Result<TokenResponse>.Failure("Çoxsaylı yanlış cəhd səbəbindən hesabınız müvəqqəti bloklanıb.", ErrorType.Conflict);

            return Result<TokenResponse>.Failure("İstifadəçi adı və ya şifrə yanlışdır.", ErrorType.Unauthorized);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = await GetUserPermissionsAsync(user.Id, ct);

        var accessToken = _jwtGenerator.GenerateAccessToken(user, roles, permissions);
        var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);

        return Result<TokenResponse>.Success(new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes)
        });
    }

    public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        var user = await _refreshTokenService.ValidateAndConsumeAsync(request.RefreshToken);

        if (user == null)
            return Result<TokenResponse>.Failure("Sessiyanın vaxtı bitib. Zəhmət olmasa, yenidən daxil olun.", ErrorType.Unauthorized);

        if (!user.IsActive)
            return Result<TokenResponse>.Failure("Hesabınız artıq aktiv deyil.", ErrorType.Forbidden);

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = await GetUserPermissionsAsync(user.Id, ct);

        var accessToken = _jwtGenerator.GenerateAccessToken(user, roles, permissions);
        var newRefreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);

        return Result<TokenResponse>.Success(new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes)
        });
    }

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(request.UserId), ct);
        if (user == null)
            return Result.Failure("İstifadəçi tapılmadı.", ErrorType.NotFound);

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);

        if (!result.Succeeded)
            return Result.Failure("Yanlış və ya vaxtı keçmiş təsdiqləmə linki.", ErrorType.ValidationError);

        return Result.Success();
    }

    public async Task<HashSet<string>> GetUserPermissionsAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user == null)
            throw new InvalidOperationException("User not found."); 

        var userRoles = await _userManager.GetRolesAsync(user);

        var rolePermissions = await _unitOfWork.ReadRepository<RolePermission, Guid>().GetAll(tracking: false)
            .Where(rp => userRoles.Contains(rp.Role.Name!))
            .Select(rp => rp.Permission.Key)
            .ToListAsync(ct);

        var userPermissions = await _unitOfWork.ReadRepository<AppUserPermission, Guid>().GetAll(tracking: false)
            .Where(up => up.UserId == user.Id)
            .Select(up => up.Permission.Key)
            .ToListAsync(ct);

        return rolePermissions.Union(userPermissions).ToHashSet();
    }
}