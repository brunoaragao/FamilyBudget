using FluentResults;

namespace Budget.Application.Errors;

public class ValidationError : Error
{
    public ValidationError(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationError(IDictionary<string, string[]> errors) : base("Validation error")
    {
        Errors = errors;
    }

    public IDictionary<string, string[]> Errors { get; }
}