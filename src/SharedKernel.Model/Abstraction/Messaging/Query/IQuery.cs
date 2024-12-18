using MediatR;
using SharedKernel.Model.Base;

namespace SharedKernel.Model.Abstraction.Messaging.Query;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

public interface ICacheQuery<TResponse> : IQuery<TResponse>, ICacheQuery;

public interface ICacheQuery
{
    string Key { get; }
    TimeSpan? Expiration { get; }
}