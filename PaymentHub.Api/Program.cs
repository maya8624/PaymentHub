using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PaymentHub.Application.Exceptions;
using PayPalIntegration.Application.Extensions;
using PayPalIntegration.Infrastructure.Persistence;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json;
using PaymentHub.Network;
using PaymentHub.Api.Extensions;
using System.Text.Json.Serialization;
using PaymentHub.Application.Interfaces;
using PaymentHub.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PayHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddServices();

// Bind PayPal settings from appsettings.json
builder.Services.Configure<PayPalSettings>(builder.Configuration.GetSection("PayPalSettings"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentHub", Version = "v1" });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandling();

app.UseCors();

app.UseHttpsRedirection();

//TODO: Remove comment when actual jwt ready 
//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
