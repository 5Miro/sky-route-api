# Domain Overview

The SkyRoute demo domain centers on turning traveler search criteria into bookable flight offers from multiple providers.

## Core Domain Areas

### Search

The search area captures the traveler intent required to request offers. The core business concept is `FlightSearchCriteria`, which includes:

- origin airport
- destination airport
- departure date
- passenger count
- cabin class

### Provider Offers

Providers return candidate flights that SkyRoute presents in a unified shape. The core concept is `FlightOffer`, representing a provider-specific flight option prepared for comparison and booking.

### Pricing

Pricing is provider-specific, but the output shown to the traveler must remain consistent. The core concept is `PriceBreakdown`, which distinguishes:

- per-passenger price
- total price for all passengers

### Route Type

Route type determines document behavior in booking. A route is treated as domestic when origin and destination are in the same country, and international when they are in different countries.

### Passengers

Passenger capture in this demo is limited to booking details for one booking form. The core concept is `PassengerDetails`, which includes:

- full name
- email address
- document number

### Booking

Booking confirms a selected offer and returns a reference code. The core concept is `BookingConfirmation`, which represents a successful booking result containing the booking reference.

## Modeling Direction

This domain should be described in a provider-neutral way wherever possible, while keeping provider-specific pricing logic separate so additional airline providers can be added later.
