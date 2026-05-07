using SkyRoute.Api.Contracts.Bookings;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Application.Interfaces;

public interface IBookingService
{
    Task<BookingConfirmation> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken);
}
