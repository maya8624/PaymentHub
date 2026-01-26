using FluentValidation;
using PaymentHub.Application.Dtos;
using PaymentHub.Application.Interfaces;
using PaymentHub.Application.Services;
using PaymentHub.Infrastructure.Interfaces;
using PaymentHub.Infrastructure.Repositories;
using PaymentHub.Network.Interfaces;
using PaymentHub.Network.Services;
using PayPalIntegration.Application.Interfaces;
using PayPalIntegration.Application.Services;
using PayPalIntegration.Infrastructure;
using PayPalIntegration.Infrastructure.Interfaces;
using PayPalIntegration.Infrastructure.Repositories;

namespace PayPalIntegration.Application.Extensions
{
    public static class ServiceExtensions
    {   
        public static void AddServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(["http://localhost:5173", "http://127.0.0.1:5500", "https://localhost:7289"])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            services.AddHttpClient();
            services.AddControllers();

            services.AddScoped<IHttpRequestSender, HttpRequestSender>();
            services.AddScoped<IPayPalAuthService, PayPalAuthService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentServcie>();
            services.AddScoped<IPayPalService, PayPalService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IRefundRepository, RefundRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();
            services.AddScoped<IValidator<RefundRequest>, RefundRequestValidator>();
            services.AddScoped<IValidator<OrderPaymentRequest>, OrderPaymentRequestValidator>();
        }
    }
}
