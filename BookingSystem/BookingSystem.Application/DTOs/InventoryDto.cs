namespace BookingSystem.Application.DTOs
{
    public class InventoryDto
    {
        public int InventoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int RemainingCount { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
}