namespace SkyRoute.Api.Domain;

public sealed record PassengerDetails(
    string FullName,
    string EmailAddress,
    string DocumentNumber);
