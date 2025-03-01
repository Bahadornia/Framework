using MediatR;

namespace Framework.Contracts.CQRS;

/// <summary>
/// Interface for MediatR commands with null response
/// </summary>
public interface ICommand: ICommand<Unit>
{}

/// <summary>
/// Interface for MediatR command with TResponse
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<out TResponse>: IRequest<TResponse> 
{ }
