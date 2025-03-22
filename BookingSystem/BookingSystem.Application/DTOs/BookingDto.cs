namespace BookingSystem.Application.DTOs
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public int MemberId { get; set; }
        public int InventoryId { get; set; }
        public DateTime BookingDate { get; set; }
    }
}