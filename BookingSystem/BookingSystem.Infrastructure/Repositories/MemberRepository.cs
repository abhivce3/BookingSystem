using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DatabaseContext _context;

        public MemberRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Member> GetMemberByIdAsync(int memberId)
        {
            return await _context.Members
                .Include(m => m.Bookings)
                .FirstOrDefaultAsync(m => m.MemberId == memberId);
        }

        public async Task BulkInsertAsync(List<Member> members)
        {
            await _context.Members.AddRangeAsync(members);
            await _context.SaveChangesAsync();
        }
    }
}