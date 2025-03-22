using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces
{
    public interface IInventoryRepository
    {
        Task<Inventory> GetInventoryByIdAsync(int inventoryId);

        Task BulkInsertAsync(List<Inventory> inventories);
    }
}