# Business Rules

This file is the primary source for rules that must remain true regardless of UI, API, or implementation details.

## Provider Pricing

### `GlobalAir`

- final price = base fare + 15% fuel surcharge
- the final price must always be rounded to 2 decimal places

### `BudgetWings`

- final price = base fare - 10% promotional discount
- the discount applies to the base fare only
- the minimum final price is `USD 29.99`

## Price Display

- the primary price shown in search results must be the total price for all passengers combined
- the per-passenger price must also be visible as secondary information
- these two values must be presented as distinct numbers, not as interchangeable labels

## Search Input

- passenger count must be between 1 and 9 inclusive
- cabin class must be one of `Economy`, `Business`, or `First Class`
- the available airport options may be hardcoded
- the airport list must include at least 6 airports across at least 2 countries

## Search Result Behavior

- search results come from mocked provider integrations in the backend
- the backend mock for each provider should return a realistic set of results for any given search
- changing sort order must not trigger another API request
- sorting must be handled on the frontend

## Booking Behavior

- confirming a booking must return a booking reference code
- the document field label and validation rule depend on route type
- domestic route uses `National ID`
- international route uses `Passport Number`

## Extensibility Constraint

- the platform is expected to onboard additional airline providers in the future
- provider-specific pricing behavior must therefore remain isolated from shared search and booking behavior
