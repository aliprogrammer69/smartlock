using FluentValidation;

using MediatR;

using SmartLock.Application.User.Commands;
using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.PipeLines {
    public sealed class ValidatorePipeLineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result {
        private readonly IEnumerable<IValidator<TRequest>> _validatores;

        public ValidatorePipeLineBehavior(IEnumerable<IValidator<TRequest>> validatores) {
            _validatores = validatores;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
            if (!_validatores.Any())
                return await next();

            string[] errors = _validatores.Select(v => v.Validate(request))
                                                     .Where(v => !v.IsValid)
                                                     .SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage)
                                                     .ToArray();


            if (errors.Any())
                return CreateResult<TResponse>(errors);

            return await next();
        }

        #region Private Methods
        private TResult CreateResult<TResult>(string[] errors) where TResult : Result {
            if (typeof(TResult) == typeof(Result))
                return Result.BadRequest(errors) as TResult;

            object resultT = typeof(Result<TResult>).GetGenericTypeDefinition()
                                                    .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
                                                    .GetMethod(nameof(Result<TResponse>.BadRequest))!
                                                    .Invoke(null, new object[] { errors })!;
            return (TResult)resultT;
        }
        #endregion
    }
}
