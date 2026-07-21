using ELibraryAPI.Domain.Constants;
using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Persistance.Data;

public static class OrderStatusSeeder
{
    public static async Task SeedOrderStatusesIfEmptyAsync(ELibraryDbContext context, CancellationToken ct = default)
    {
        if (await context.OrderStatuses.AnyAsync(ct))
            return;

        var now = DateTime.UtcNow;
        var statuses = new List<OrderStatus>
        {
            new() { Id = Guid.NewGuid(), Name = OrderStatusNames.Pending,    CreatedDate = now },
            new() { Id = Guid.NewGuid(), Name = OrderStatusNames.Processing,  CreatedDate = now },
            new() { Id = Guid.NewGuid(), Name = OrderStatusNames.Completed,   CreatedDate = now },
            new() { Id = Guid.NewGuid(), Name = OrderStatusNames.Cancelled,   CreatedDate = now },
        };

        await context.OrderStatuses.AddRangeAsync(statuses, ct);
        await context.SaveChangesAsync(ct);
    }
}
