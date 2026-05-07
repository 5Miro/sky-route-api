namespace SkyRoute.Api.Application.Exceptions;

public sealed class RequestValidationException : Exception
{
    public RequestValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = new Dictionary<string, string[]>(errors, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}
