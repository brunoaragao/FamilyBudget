using Budget.Application.Errors;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Any())
        {
            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(ResultBase)Result.Fail(failures.Select(x => x.ErrorMessage));
            }

            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var errorType = typeof(TResponse).GetGenericArguments()[0];

                var dictionary = failures
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToArray());

                var error = new ValidationError(dictionary);

                var failMethod = typeof(Result).GetMethods()
                    .Where(x => x.Name == nameof(Result.Fail))
                    .Where(x => x.IsGenericMethod)
                    .Where(x => x.GetParameters().Length == 1)
                    .Where(x => x.GetParameters()[0].ParameterType == typeof(IError))
                    .Single()
                    .MakeGenericMethod(errorType);

                return (TResponse)(ResultBase)failMethod.Invoke(null, new[] { error })!;
            }
        }

        return await next();
    }
}