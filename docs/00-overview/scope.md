# Scope

## In Scope

This demo includes the minimum product slice explicitly required by the challenge brief:

- a flight search form
- mocked flight search results from `GlobalAir` and `BudgetWings`
- frontend result display including provider, flight number, departure time, arrival time, duration, cabin class, and price
- frontend sorting of results by price, duration, and departure time
- loading and empty states for flight search
- a booking screen for a selected flight
- a passenger details form with full name, email address, and route-dependent document number
- a confirm booking action that returns a booking reference code
- a backend API that supports search and booking flows

## Required Constraints

- airport choices are hardcoded
- at least 6 airports must be available
- the airports must cover at least 2 countries
- airline providers are mocked in the backend
- each provider mock should return realistic flight results for any given search

## Out of Scope

The brief does not require the following, so they are excluded from this first-pass scope:

- cloud deployment
- real airline provider integrations
- live airport lookup or autocomplete
- payment processing
- seat maps or seat selection
- booking management after confirmation
- cancellation, refund, or change flows
- authentication or user accounts
- notifications, emails, or invoices
- any feature not directly needed to search, compare, and book a flight locally

## Delivery Boundary

Running locally is sufficient. If time is constrained later, incomplete work should be documented in the README rather than expanding scope inside these product docs.
