using System.Text.Json.Serialization;
using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Application.Services;
using SkyRoute.Api.Infrastructure.Bookings;
using SkyRoute.Api.Infrastructure.Configuration;
using SkyRoute.Api.Infrastructure.Lookups;
using SkyRoute.Api.Infrastructure.Offers;
using SkyRoute.Api.Infrastructure.Providers;
using SkyRoute.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CorsOptions>(builder.Configuration.GetSection(CorsOptions.SectionName));
builder.Services.Configure<OfferStoreOptions>(builder.Configuration.GetSection(OfferStoreOptions.SectionName));

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        CorsOptions.PolicyName,
        policy =>
        {
            var allowedOrigins = builder.Configuration
                .GetSection(CorsOptions.SectionName)
                .Get<CorsOptions>()?.AllowedOrigins
                ?.Where(origin => !string.IsNullOrWhiteSpace(origin))
                .ToArray();

            if (allowedOrigins is { Length: > 0 })
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
            else
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
        });
});

builder.Services.AddSingleton<IAirportCatalog, InMemoryAirportCatalog>();
builder.Services.AddSingleton<IDocumentRuleService, DocumentRuleService>();
builder.Services.AddSingleton<IOfferStore, InMemoryOfferStore>();
builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddScoped<IFlightSearchService, FlightSearchService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IFlightProviderClient, GlobalAirFlightProviderClient>();
builder.Services.AddScoped<IFlightProviderClient, BudgetWingsFlightProviderClient>();

var app = builder.Build();

app.UseMiddleware<ApiExceptionMiddleware>();

app.UseCors(CorsOptions.PolicyName);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
