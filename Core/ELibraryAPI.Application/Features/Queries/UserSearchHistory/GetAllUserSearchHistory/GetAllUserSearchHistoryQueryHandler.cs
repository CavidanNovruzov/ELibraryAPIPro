using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.UserSearchHistory.GetAllUserSearchHistory;

public sealed class GetAllUserSearchHistoryQueryHandler : IRequestHandler<GetAllUserSearchHistoryQueryRequest, Result<GetAllUserSearchHistoryQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetAllUserSearchHistoryQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetAllUserSearchHistoryQueryResponse>> Handle(GetAllUserSearchHistoryQueryRequest request, CancellationToken cancellationToken)
    {
        Guid currentUserId = Guid.Parse(_currentUserService.UserId.ToString());

        var histories = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.UserSearchHistory, Guid>()
            .GetAll(tracking: false)
            .Where(x => x.UserId == currentUserId)
            .OrderByDescending(x => x.CreatedDate)
            .Take(15)
            .Select(x => new UserSearchHistoryListDto(
                x.Id,
                x.UserId,
                x.SearchQuery,
                x.CreatedDate
            ))
            .ToListAsync(cancellationToken);

        return Result<GetAllUserSearchHistoryQueryResponse>.Success(new GetAllUserSearchHistoryQueryResponse(histories));
    }
}