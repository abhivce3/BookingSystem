namespace BookingSystem.Application.DTOs
{
    public class MemberDto
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BookingCount { get; set; }
        public DateTime DateJoined { get; set; }
    }
}