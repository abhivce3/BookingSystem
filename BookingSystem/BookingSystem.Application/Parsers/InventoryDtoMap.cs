using CsvHelper.Configuration;
using BookingSystem.Application.DTOs;

namespace BookingSystem.Application.Parsers
{
    public class InventoryDtoMap : ClassMap<InventoryDto>
    {
        public InventoryDtoMap()
        {
            Map(m => m.Title).Name("title"); // Maps "title" column
            Map(m => m.Description).Name("description"); // Maps "title" column
            Map(m => m.RemainingCount).Name("remaining_count"); // Maps "remaining_count" column
            Map(m => m.ExpirationDate).Name("expiration_date") // ✅ Define correct DateTime format
            .TypeConverterOption.Format("dd/MM/yyyy"); ; // Maps "remaining_count" column
        }
    }
}
