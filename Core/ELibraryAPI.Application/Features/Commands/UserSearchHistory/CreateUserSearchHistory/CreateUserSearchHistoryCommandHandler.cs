using AutoMapper;
using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserSearchHistory.CreateUserSearchHistory;

public sealed class CreateUserSearchHistoryCommandHandler : IRequestHandler<CreateUserSearchHistoryCommandRequest, Result<CreateUserSearchHistoryCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateUserSearchHistoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CreateUserSearchHistoryCommandResponse>> Handle(CreateUserSearchHistoryCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (!userId.HasValue)
            return Result<CreateUserSearchHistoryCommandResponse>.Failure("Sistemə daxil olunmamışdır.", ErrorType.Unauthorized);

        var normalizedQuery = request.SearchQuery?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedQuery))
        {
            return Result<CreateUserSearchHistoryCommandResponse>.Failure("Axtarış sorğusu boş ola bilməz.", ErrorType.ValidationError);
        }

        var historyWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.UserSearchHistory, Guid>();

        var searchHistory = _mapper.Map<Domain.Entities.Concrete.UserSearchHistory>(request);
        searchHistory.SearchQuery = normalizedQuery;

        searchHistory.UserId = userId.Value;

        await historyWriteRepository.AddAsync(searchHistory, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateUserSearchHistoryCommandResponse>.Success(
            new CreateUserSearchHistoryCommandResponse(searchHistory.Id),
            "Axtarış tarixçəsi uğurla saxlanıldı.");
    }
}