# Vision

SkyRoute is a travel aggregator platform that helps users search, compare, and book flights from multiple airline providers through a single experience.

This demo project implements a focused product slice: Flight Search and Booking. The goal is to let a traveler:

- choose an origin and destination airport
- choose a departure date
- choose a passenger count from 1 to 9
- choose a cabin class
- compare flight offers from multiple providers
- select one offer and complete a booking

The demo should show production-quality thinking in both frontend and backend design while staying narrow enough to build locally within the challenge scope.

For this first pass, SkyRoute is expected to aggregate offers from two mocked providers:

- `GlobalAir`
- `BudgetWings`

The experience should make price information clear, especially the difference between:

- total price for all passengers
- per-passenger price as secondary information

The system must also adapt the passenger document requirement to the selected route:

- domestic route: `National ID`
- international route: `Passport Number`

The long-term product expectation is that SkyRoute will onboard additional airline providers in the future, so this demo should use domain language and documentation that can grow beyond the initial two providers.
