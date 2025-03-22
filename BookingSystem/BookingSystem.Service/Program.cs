using BookingSystem.Application;
using BookingSystem.Infrastructure;
using BookingSystem.Service.Services;
using BookingSystem.Service.Behaviors;
using BookingSystem.Service.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add gRPC Services
builder.Services.AddGrpc();


// ✅ Register gRPC Reflection
builder.Services.AddGrpcReflection();

// ✅ Configure Authentication (JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://your-identity-provider/";
        options.Audience = "booking-api";
    });

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(GrpcResponsePipelineBehavior<,>));




var app = builder.Build();

// ✅ Use Middleware
app.UseExceptionHandlingMiddleware();
app.UseAuthentication();
//app.UseAuthorization();
//app.UseAuthenticationMiddleware();

// ✅ Map gRPC Services
app.MapGrpcService<BookingService>();
app.MapGrpcService<InventoryService>();
app.MapGrpcService<MemberService>();


if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGet("/", () => "Use a gRPC client to communicate.");

app.Run();