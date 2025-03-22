using CsvHelper.Configuration;
using BookingSystem.Application.DTOs;

namespace BookingSystem.Application.Parsers
{
    public class MemberDtoMap : ClassMap<MemberDto>
    {
        public MemberDtoMap()
        {
            Map(m => m.FirstName).Name("name"); // Maps "name" to Member
            Map(m => m.LastName).Name("surname"); // Maps "surname" column
            Map(m => m.BookingCount).Name("booking_count"); // Maps "booking_count" column
            Map(m => m.DateJoined).Name("date_joined"); // Maps "date_joined" column

        }
    }
}
