using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Tag.GetAllTag;

public sealed record GetAllTagQueryRequest(int Page = 1, int Size = 50) : IRequest<Result<GetAllTagQueryResponse>>;
