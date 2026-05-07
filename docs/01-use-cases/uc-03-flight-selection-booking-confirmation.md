# UC-03 Flight Selection and Booking Confirmation

## Goal

Allow a traveler to select one flight offer, review its details, provide passenger information, and confirm a booking.

## Actors

- traveler
- SkyRoute frontend
- SkyRoute backend

## Preconditions

- flight results are available
- the traveler has selected one flight offer

## Trigger

The traveler proceeds from search results into the booking screen.

## Main Flow

1. The frontend displays the selected flight summary.
2. The frontend displays the price breakdown for the selected offer.
3. The traveler enters full name, email address, and document number.
4. The traveler confirms the booking.
5. The frontend submits the booking request to the backend.
6. The backend creates a booking response and returns a booking reference code.
7. The frontend displays the returned booking reference code.

## Alternate Flows

1. If passenger details are incomplete or invalid, the booking cannot be confirmed until the form is corrected.

## Postconditions

- a confirmed booking reference code is available to the traveler

## Business Rules Touched

- price display rules in [Business Rules](../00-overview/business-rules.md)
- booking confirmation rule in [Business Rules](../00-overview/business-rules.md)
- route-dependent document behavior in [Business Rules](../00-overview/business-rules.md)
