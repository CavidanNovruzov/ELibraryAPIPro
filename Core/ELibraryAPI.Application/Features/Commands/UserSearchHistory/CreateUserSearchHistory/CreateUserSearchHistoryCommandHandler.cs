using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserSearchHistory.CreateUserSearchHistory;

public sealed class CreateUserSearchHistoryCommandHandler : IRequestHandler<CreateUserSearchHistoryCommandRequest, Result<CreateUserSearchHistoryCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserSearchHistoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateUserSearchHistoryCommandResponse>> Handle(CreateUserSearchHistoryCommandRequest request, CancellationToken ct)
    {
        var historyWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.UserSearchHistory, Guid>();
        var userReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Auth.AppUser, Guid>();

        var userExists = await userReadRepository.ExistsAsync(x => x.Id == request.UserId, false, ct);
        if (!userExists)
        {
            return Result<CreateUserSearchHistoryCommandResponse>.Failure("User not found.");
        }

        var normalizedQuery = request.SearchQuery?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedQuery))
        {
            return Result<CreateUserSearchHistoryCommandResponse>.Failure("Search query cannot be empty.");
        }

        var searchHistory = _mapper.Map<Domain.Entities.Concrete.UserSearchHistory>(request);
        searchHistory.SearchQuery = normalizedQuery;

        await historyWriteRepository.AddAsync(searchHistory, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateUserSearchHistoryCommandResponse>.Success(
            new CreateUserSearchHistoryCommandResponse(searchHistory.Id),
            "Search history record created successfully.");
    }
}