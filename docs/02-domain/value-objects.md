# Value Objects

## Airport

Represents a selectable airport in the hardcoded airport list.

Expected meaning:

- airport code or equivalent identifier
- display name
- country

## Cabin Class

Represents the travel class selected for search and displayed in offers.

Allowed values:

- `Economy`
- `Business`
- `First Class`

## Passenger Count

Represents the number of passengers in the search.

Allowed range:

- integer from 1 to 9 inclusive

## Money

Represents a monetary amount used in pricing display and booking summaries.

For this challenge, the brief expresses price examples in `USD`.

## Flight Number

Represents the provider-issued identifier shown in search results.

## Booking Reference

Represents the confirmation code returned after a successful booking.

## Document Number

Represents the route-dependent traveler document captured during booking.

Its meaning changes with route type:

- domestic route: national ID number
- international route: passport number

## Price Breakdown

Represents the pricing values shown to the traveler for a selected offer.

Core parts:

- per-passenger price
- passenger count
- total price
