using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Infrastructure.Options;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;


namespace ELibraryAPI.Application.Features.Commands.Auth.ForgotPassword;

public sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommandRequest, Result>
{
    private readonly UserManager<Domain.Entities.Concrete.Auth.AppUser> _userManager;
    private readonly IEmailSender        _emailSender;
    private readonly SmtpOptions _smtpOptions;
    public ForgotPasswordCommandHandler(UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager, IEmailSender emailSender, IOptions<SmtpOptions> smtpOptions)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _smtpOptions = smtpOptions.Value;
    }

    public async Task<Result> Handle(ForgotPasswordCommandRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        // Güvənlik səbəbindən həmişə eyni cavabı qaytar (user enumeration-ın qarşısını al)
        if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            return Result.Success("If the email exists, a reset link has been sent.");

        var token      = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Uri.EscapeDataString(token);
        var resetLink = $"{_smtpOptions.ResetPasswordBaseUrl}?userId={user.Id}&token={encodedToken}";

        await _emailSender.SendEmailAsync(
            to:      user.Email!,
            subject: "Libraff.az — Şifrə sıfırlama",
            htmlBody: $"Şifrənizi sıfırlamaq üçün <a href='{resetLink}'>bura klikləyin</a>.",
            ct: ct);

        return Result.Success("If the email exists, a reset link has been sent.");
    }
}
