using FluentValidation;
using PaymentHub.Application.Dtos;
using PaymentHub.Application.Interfaces;
using PaymentHub.Application.Services;
using PaymentHub.Network.Interfaces;
using PaymentHub.Network.Services;
using PayPalIntegration.Application.Interfaces;
using PayPalIntegration.Application.Services;
using PayPalIntegration.Domain.Interfaces;
using PayPalIntegration.Infrastructure;
using PayPalIntegration.Infrastructure.Repositories;

namespace PayPalIntegration.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddService(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            services.AddHttpClient();
            services.AddControllers();

            services.AddScoped<IPayPalAuthService, PayPalAuthService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();
            //services.AddScoped<IValidator<CustomerRequest>, CustomerValidator>();
            //services.AddScoped<IValidator<LogInRequest>, LogInValidator>();
            //services.AddScoped<IValidator<MenuRequest>, MenuRequestValidator>();
            //services.AddScoped<IValidator<MenuPatchRequest>, MenuPatchRequestValidator>();
            //services.AddScoped<IValidator<MessageRequest>, MessageValidator>();
            //services.AddScoped<IValidator<RegisterRequest>, RegisterValidator>();
        }
    }
}
