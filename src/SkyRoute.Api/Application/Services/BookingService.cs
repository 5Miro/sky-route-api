using System.ComponentModel.DataAnnotations;
using SkyRoute.Api.Application.Exceptions;
using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Contracts.Bookings;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Application.Services;

public sealed class BookingService : IBookingService
{
    private readonly IDocumentRuleService _documentRuleService;
    private readonly IBookingRepository _bookingRepository;
    private readonly IOfferStore _offerStore;

    public BookingService(
        IDocumentRuleService documentRuleService,
        IBookingRepository bookingRepository,
        IOfferStore offerStore)
    {
        _documentRuleService = documentRuleService;
        _bookingRepository = bookingRepository;
        _offerStore = offerStore;
    }

    public async Task<BookingConfirmation> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(request.OfferId))
        {
            errors[nameof(request.OfferId)] = ["Offer ID is required."];
        }

        if (request.PassengerDetails is null)
        {
            errors[nameof(request.PassengerDetails)] = ["Passenger details are required."];
        }

        if (errors.Count > 0)
        {
            throw new RequestValidationException(errors);
        }

        var offer = _offerStore.Get(request.OfferId);
        if (offer is null)
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                [nameof(request.OfferId)] = ["Offer ID is unknown or has expired. Please search again."]
            });
        }

        var passengerDetails = ValidatePassengerDetails(request.PassengerDetails!, offer.RouteType);
        var confirmation = new BookingConfirmation(
            BookingReference: GenerateBookingReference(),
            OfferId: offer.OfferId,
            Provider: offer.Provider,
            Origin: offer.Origin,
            Destination: offer.Destination,
            CabinClass: offer.CabinClass,
            PassengerCount: offer.PriceBreakdown.PassengerCount,
            PriceBreakdown: offer.PriceBreakdown);

        await _bookingRepository.SaveAsync(confirmation, cancellationToken);
        return confirmation;
    }

    private PassengerDetails ValidatePassengerDetails(PassengerDetailsDto details, RouteType routeType)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(details.FullName))
        {
            errors[nameof(details.FullName)] = ["Full name is required."];
        }

        if (string.IsNullOrWhiteSpace(details.EmailAddress))
        {
            errors[nameof(details.EmailAddress)] = ["Email address is required."];
        }
        else
        {
            var emailValidator = new EmailAddressAttribute();
            if (!emailValidator.IsValid(details.EmailAddress))
            {
                errors[nameof(details.EmailAddress)] = ["Email address must be valid."];
            }
        }

        var rule = _documentRuleService.GetRule(routeType);
        if (!_documentRuleService.TryNormalizeAndValidate(rule, details.DocumentNumber, out var normalizedDocumentNumber, out var documentError))
        {
            errors[nameof(details.DocumentNumber)] = [documentError];
        }

        if (errors.Count > 0)
        {
            throw new RequestValidationException(errors);
        }

        return new PassengerDetails(
            FullName: details.FullName.Trim(),
            EmailAddress: details.EmailAddress.Trim(),
            DocumentNumber: normalizedDocumentNumber);
    }

    private static string GenerateBookingReference()
    {
        const string alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        Span<char> buffer = stackalloc char[8];

        for (var index = 0; index < buffer.Length; index++)
        {
            buffer[index] = alphabet[Random.Shared.Next(alphabet.Length)];
        }

        return $"SR-{new string(buffer)}";
    }
}
