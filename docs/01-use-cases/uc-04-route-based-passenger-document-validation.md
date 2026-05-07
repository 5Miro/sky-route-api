# UC-04 Route-Based Passenger Document Validation

## Goal

Ensure the passenger document field reflects the selected route type.

## Actors

- traveler
- SkyRoute frontend

## Preconditions

- the traveler is on the booking screen
- a flight offer with a known origin and destination has been selected

## Trigger

The booking screen is shown for the selected route.

## Main Flow

1. The frontend determines whether the route is domestic or international.
2. If the route is domestic, the document field is labeled `National ID`.
3. If the route is international, the document field is labeled `Passport Number`.
4. The frontend applies the matching validation rule to the field.
5. The traveler provides a document number in the expected format.

## Alternate Flows

1. If the traveler enters a document number that does not satisfy the current rule, the form remains invalid until corrected.

## Postconditions

- the booking form uses the correct document label and validation behavior for the selected route

## Business Rules Touched

- route-dependent document behavior in [Business Rules](../00-overview/business-rules.md)
