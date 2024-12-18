using MediatR;
using SharedKernel.Model.Base;

namespace SharedKernel.Model.Abstraction.Messaging.Command;

public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : ICommand;

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : ICommand<TResponse>
    where TResponse : class;