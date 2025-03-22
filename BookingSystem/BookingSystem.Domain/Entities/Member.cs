namespace BookingSystem.Domain.Entities
{
    public class Member
    {
        public int MemberId { get; set; }  // Primary Key
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BookingCount { get; set; }

        public DateTime DateJoined { get; set; }
        // A Member can have multiple Bookings (One-to-Many)
        public ICollection<Booking> Bookings { get; set; }
    }
}