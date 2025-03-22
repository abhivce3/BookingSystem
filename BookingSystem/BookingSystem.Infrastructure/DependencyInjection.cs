using BookingSystem.Application.Config;
using BookingSystem.Application.Interfaces;
using BookingSystem.Infrastructure.Persistence;
using BookingSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)));

            // ✅ Bind BookingConfig to appsettings
            services.Configure<BookingConfig>(configuration.GetSection("BookingSettings"));

            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();

            return services;
        }
    }
}