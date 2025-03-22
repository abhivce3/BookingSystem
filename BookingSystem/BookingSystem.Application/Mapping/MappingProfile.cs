using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ✅ Entity → DTO Mapping
            CreateMap<Booking, BookingDto>();
            CreateMap<Member, MemberDto>();
            CreateMap<Inventory, InventoryDto>();

            // ✅ DTO → Entity Mapping
            CreateMap<BookingDto, Booking>();
            CreateMap<MemberDto, Member>();
            CreateMap<InventoryDto, Inventory>();
        }
    }
}