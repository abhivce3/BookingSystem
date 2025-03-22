namespace BookingSystem.Domain.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }  // Primary Key

        // Foreign Key: Member
        public int MemberId { get; set; }

        public Member Member { get; set; }

        // Foreign Key: Inventory
        public int InventoryId { get; set; }

        public Inventory Inventory { get; set; }

        public bool IsCancelled { get; set; } = false;

        public DateTime BookingDate { get; set; } // Timestamp of booking
    }
}