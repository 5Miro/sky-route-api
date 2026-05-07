using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Application.Interfaces;

public interface IBookingRepository
{
    Task SaveAsync(BookingConfirmation bookingConfirmation, CancellationToken cancellationToken);
}
