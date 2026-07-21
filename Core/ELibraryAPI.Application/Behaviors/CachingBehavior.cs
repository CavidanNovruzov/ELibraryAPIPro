using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ELibraryAPI.Application.Behaviors;

public sealed class CachingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : class
{
    private readonly ICacheService _cache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(ICacheService cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICacheable cacheable)
            return await next();

        var cached = await _cache.GetAsync<TResponse>(cacheable.CacheKey, cancellationToken);
        if (cached is not null)
        {
            _logger.LogInformation("Cache HIT for key: {Key}", cacheable.CacheKey);
            return cached;
        }

        _logger.LogInformation("Cache MISS for key: {Key}. Fetching from database...", cacheable.CacheKey);
        var response = await next();

        if (response is Result r)
        {
            if (r.IsSuccess)
            {
                await _cache.SetAsync(
                    cacheable.CacheKey,
                    response,
                    cacheable.AbsoluteExpiration,
                    cacheable.SlidingExpiration,
                    cancellationToken);

                _logger.LogInformation("Cache SET (Result) for key: {Key}", cacheable.CacheKey);
            }
        }
        else if (response is not null)
        {
            await _cache.SetAsync(
                cacheable.CacheKey,
                response,
                cacheable.AbsoluteExpiration,
                cacheable.SlidingExpiration,
                cancellationToken);

            _logger.LogInformation("Cache SET (Standard Object) for key: {Key}", cacheable.CacheKey);
        }

        return response;
    }
}