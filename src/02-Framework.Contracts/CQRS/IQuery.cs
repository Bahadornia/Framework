using MediatR;

namespace App.Framework.Contracts.CQRS;

public interface IQuery<out TResponse>: IRequest<TResponse>
    where TResponse : notnull
{
}
