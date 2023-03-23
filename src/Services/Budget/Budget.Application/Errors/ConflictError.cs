using FluentResults;

namespace Budget.Application.Errors;

public class ConflictError : Error
{
    public ConflictError(string message) : base(message)
    {
    }
}