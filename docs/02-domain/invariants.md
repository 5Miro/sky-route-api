# Invariants

## Search Invariants

- passenger count is always between 1 and 9 inclusive
- cabin class is always one of `Economy`, `Business`, or `First Class`
- origin and destination must be chosen from the supported airport list

## Pricing Invariants

- every displayed flight offer has provider-derived pricing
- total price and per-passenger price must remain distinguishable in the UI and booking flow
- total price must correspond to the selected passenger count

## Provider Invariants

- `GlobalAir` pricing always includes a 15% fuel surcharge and rounds the final result to 2 decimals
- `BudgetWings` pricing always applies a 10% discount to base fare only
- `BudgetWings` final price never drops below `USD 29.99`

## Route Invariants

- route type depends on whether origin and destination are in the same country
- domestic routes require `National ID` behavior
- international routes require `Passport Number` behavior

## Booking Invariants

- booking confirmation returns a booking reference code
- a booking is based on one selected flight offer
- passenger details must satisfy the active document rule for the selected route
