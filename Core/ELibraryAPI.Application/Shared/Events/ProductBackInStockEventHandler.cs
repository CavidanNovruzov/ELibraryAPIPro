using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ELibraryAPI.Application.Features.Events;

public sealed class ProductBackInStockEventHandler : INotificationHandler<ProductBackInStockEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender; 
    private readonly ILogger<ProductBackInStockEventHandler> _logger;

    public ProductBackInStockEventHandler(
        IUnitOfWork unitOfWork,
        IEmailSender emailSender,
        ILogger<ProductBackInStockEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task Handle(ProductBackInStockEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Product {ProductId} is back in stock. Processing notifications...", notification.ProductId);

        var wishlistReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.WishlistItem, Guid>();

        var wishlistItems = await wishlistReadRepo.GetAll(tracking: true)
            .Include(w => w.Wishlist)
                .ThenInclude(w => w.User)
            .Include(w => w.Product)
            .Where(w => w.ProductId == notification.ProductId && w.NotifyWhenAvailable)
            .ToListAsync(ct);

        if (!wishlistItems.Any())
            return;

        foreach (var item in wishlistItems)
        {
            try
            {
                var userEmail = item.Wishlist.User.Email;
                var userName = item.Wishlist.User.UserName;
                var productTitle = item.Product.Title;

                if (!string.IsNullOrEmpty(userEmail))
                {
            
                    await _emailSender.SendEmailAsync(
                        to: userEmail,
                        subject: "Məhsul yenidən stokda!",
                        htmlBody: $"Salam {userName}, gözlədiyiniz '{productTitle}' məhsulu artıq stokdadır. Bitmədən sifariş edə bilərsiniz.");
                }

                item.NotifyWhenAvailable = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send stock notification email to {Email}", item.Wishlist.User.Email);
            }
        }

        await _unitOfWork.SaveAsync(ct);
    }
}