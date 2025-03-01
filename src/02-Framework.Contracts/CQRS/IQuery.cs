using MediatR;

namespace Framework.Contracts.CQRS;

public interface IQuery<out TResponse>: IRequest<TResponse>
    where TResponse : notnull
{
}
