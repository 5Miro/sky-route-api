# UC-01 Flight Search Submission

## Goal

Allow a traveler to search for available flights using the required search criteria.

## Actors

- traveler
- SkyRoute frontend
- SkyRoute backend
- mocked providers: `GlobalAir`, `BudgetWings`

## Preconditions

- the user is on the flight search screen
- airport options are available
- the user can choose a valid passenger count and cabin class

## Trigger

The traveler submits the search form.

## Main Flow

1. The traveler selects origin, destination, departure date, passenger count, and cabin class.
2. The traveler submits the form.
3. The frontend sends the search request to the backend.
4. The backend gathers mocked offers from the available providers.
5. The backend returns flight offers to the frontend.
6. The frontend displays the returned results, including total and per-passenger price information.

## Alternate Flows

1. While the request is in progress, the frontend shows a loading indicator.
2. If no flights match the search, the frontend shows a clear empty state.

## Postconditions

- the traveler sees matching flight offers or a clear empty state
- the result set is available for frontend sorting and flight selection

## Business Rules Touched

- search input limits in [Business Rules](../00-overview/business-rules.md)
- price display rules in [Business Rules](../00-overview/business-rules.md)
- mocked provider result behavior in [Business Rules](../00-overview/business-rules.md)
