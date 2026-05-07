using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Infrastructure.Bookings;

public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly List<BookingConfirmation> _bookings = [];
    private readonly object _lock = new();

    public Task SaveAsync(BookingConfirmation bookingConfirmation, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            _bookings.Add(bookingConfirmation);
        }

        return Task.CompletedTask;
    }
}
