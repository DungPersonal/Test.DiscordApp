using MediatR;
using SharedKernel.Model.Base;

namespace SharedKernel.Model.Abstraction.Messaging.Command;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
