using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
            .HasKey(m => m.MemberId); // Primary Key

            modelBuilder.Entity<Member>()
                .Property(m => m.MemberId)
                .ValueGeneratedOnAdd(); // ✅ Identity column (auto-increment)

            modelBuilder.Entity<Member>()
                .Property(m => m.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Member>()
                .Property(m => m.LastName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Member>()
           .Property(b => b.BookingCount)
           .HasDefaultValue(0);  // ✅ Default value is `0`

            modelBuilder.Entity<Member>()
               .Property(m => m.DateJoined)
               .HasDefaultValueSql("GETUTCDATE()")  // ✅ Default value is current UTC datetime
               .IsRequired();

            modelBuilder.Entity<Member>()
                .HasMany(m => m.Bookings)
                .WithOne(b => b.Member)
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Configure Inventory Entity
            modelBuilder.Entity<Inventory>()
                .HasKey(i => i.InventoryId);

            modelBuilder.Entity<Inventory>()
              .Property(m => m.InventoryId)
              .ValueGeneratedOnAdd(); // ✅ Identity column (auto-increment)

            modelBuilder.Entity<Inventory>()
                .Property(i => i.Title)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Inventory>()
               .Property(i => i.Description)
               .HasMaxLength(500)
                .IsRequired(false);  // ✅ Allows null values

            // ✅ Configure Inventory Entity
            modelBuilder.Entity<Inventory>()
                .Property(i => i.ExpirationDate)
                .IsRequired(false);  // ✅ Allows null values

            modelBuilder.Entity<Inventory>()
                .Property(i => i.RemainingCount)
                .IsRequired()
                .HasDefaultValue(0);

            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.Bookings)
                .WithOne(b => b.Inventory)
                .HasForeignKey(b => b.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Configure Booking Entity
            modelBuilder.Entity<Booking>()
                .HasKey(b => b.BookingId);

            modelBuilder.Entity<Booking>()
            .Property(m => m.BookingId)
            .ValueGeneratedOnAdd(); // ✅ Identity column (auto-increment)

            // ✅ Configure Booking Entity
            modelBuilder.Entity<Booking>()
                .Property(b => b.BookingDate)
                .HasDefaultValueSql("GETUTCDATE()");  // ✅ Default value is current UTC datetime

            modelBuilder.Entity<Booking>()
            .Property(b => b.IsCancelled)
            .HasDefaultValue(false);  // ✅ Default value is `false`

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Member)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MemberId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Inventory)
                .WithMany(i => i.Bookings)
                .HasForeignKey(b => b.InventoryId);
        }
    }
}