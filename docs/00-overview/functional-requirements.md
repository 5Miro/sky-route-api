# Functional Requirements

## Search

The application must provide a flight search form that captures:

- origin airport
- destination airport
- departure date
- passenger count from 1 to 9
- cabin class: `Economy`, `Business`, or `First Class`

The airport list may be hardcoded. No live airport API is required.

## Search Results

When the user submits a valid search, the frontend must call the backend and display matching flight results.

Each result must show:

- airline provider
- flight number
- departure time
- arrival time
- duration
- cabin class
- total price
- per-passenger price as secondary information

The price presentation must clearly distinguish the combined total from the single-passenger amount.

## Result States

The application must support these result states:

- loading state while search is in progress
- populated state when flights are returned
- empty state when no flights match the search

## Sorting

The user must be able to sort results by:

- price, low to high
- price, high to low
- duration, shortest first
- departure time

Sorting must happen on the frontend without making another API request.

## Booking Flow

When the user selects a flight, the application must show a booking screen containing:

- a summary of the selected flight
- a price breakdown
- a passenger details form
- a confirm booking action

The flight summary must include:

- route
- provider
- departure and arrival times
- cabin class

The price breakdown must include:

- per-passenger price
- number of passengers
- total price

The passenger details form must include:

- full name
- email address
- document number

## Route-Dependent Document Requirement

The document number field must change based on whether the selected route is domestic or international:

- domestic route: label the field `National ID`
- international route: label the field `Passport Number`

The validation rule must also change with the label. The brief requires the adaptation, but it does not define the exact validation format.

## Booking Confirmation

When the user confirms a booking, the frontend must submit the booking to the backend and receive a booking reference code in response.

## Backend Support

The backend must provide API support for:

- searching flights
- returning mocked provider results
- confirming a booking
- returning a booking reference code

These docs intentionally describe business behavior only. Detailed endpoint shapes and transport contracts are deferred.
