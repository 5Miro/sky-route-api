# UC-02 Flight Results Sorting

## Goal

Allow a traveler to reorder returned flight results without re-running the search.

## Actors

- traveler
- SkyRoute frontend

## Preconditions

- a set of flight results is already displayed

## Trigger

The traveler changes the selected sort option.

## Main Flow

1. The traveler chooses a sort option.
2. The frontend reorders the existing result set.
3. The frontend updates the displayed order.

## Alternate Flows

1. If there are no results, sorting is not applicable and the empty state remains visible.

## Postconditions

- the same result set remains visible in a new order
- no additional backend search request is made

## Business Rules Touched

- frontend-only sorting in [Business Rules](../00-overview/business-rules.md)
