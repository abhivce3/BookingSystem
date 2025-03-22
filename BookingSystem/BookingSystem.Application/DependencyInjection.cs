using FluentValidation;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using BookingSystem.Application.Features.Booking.Validators;
using MediatR;
using BookingSystem.Application.Features.Inventory.Validators;
using BookingSystem.Application.Features.Member.Validators;

namespace BookingSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());  // ✅ This registers all validators from the current assembly.

            return services;
        }
    }
}