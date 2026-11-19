namespace RoomHub.Api.Common;

public interface ICommandHandler<in TCommand, TResult>
    where TResult : Result
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}
