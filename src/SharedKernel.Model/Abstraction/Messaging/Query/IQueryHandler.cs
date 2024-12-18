using MediatR;
using SharedKernel.Model.Base;

namespace SharedKernel.Model.Abstraction.Messaging.Query;

public interface IQueryHandler<in TRequest, TResponse>: IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IQuery<TResponse>
    where TResponse : class;