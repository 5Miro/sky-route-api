# Entities

## Flight Offer

`FlightOffer` is the main comparison and selection entity in the search flow.

Suggested identity:

- provider
- flight number
- route
- departure time

Core data carried by the entity:

- arrival time
- duration
- cabin class
- price breakdown

## Provider

`Provider` represents a flight source integrated into SkyRoute.

Initial instances:

- `GlobalAir`
- `BudgetWings`

The provider determines which pricing rule is applied to its base fare.

## Route

`Route` represents the travel path for a flight offer or booking context.

Core data:

- origin airport
- destination airport

Route also determines whether the trip is domestic or international for document handling.

## Booking

`Booking` represents the confirmation of a selected flight offer using passenger-entered details.

Core data:

- selected flight offer
- passenger details
- booking reference

## Passenger Details

`PassengerDetails` captures the traveler information required by the booking form in this challenge.

Core data:

- full name
- email address
- document number

This entity is intentionally narrow because the brief does not require multiple passengers with separate forms, loyalty data, or payment data.
