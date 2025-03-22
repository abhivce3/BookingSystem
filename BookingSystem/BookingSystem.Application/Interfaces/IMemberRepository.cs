using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetMemberByIdAsync(int memberId);

        Task BulkInsertAsync(List<Member> members);
    }
}