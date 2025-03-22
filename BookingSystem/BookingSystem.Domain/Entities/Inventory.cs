namespace BookingSystem.Domain.Entities
{
    public class Inventory
    {
        public int InventoryId { get; set; }  // Primary Key
        public string Title { get; set; }
        public string Description { get; set; }
        public int RemainingCount { get; set; } // Number of available items
        public DateTime? ExpirationDate { get; set; }

        // An Inventory item can have multiple bookings (One-to-Many)
        public ICollection<Booking> Bookings { get; set; }
    }
}