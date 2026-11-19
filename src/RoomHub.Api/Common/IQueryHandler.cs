namespace RoomHub.Api.Common;

public interface IQueryHandler<in TQuery, TResult>
    where TResult : Result
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}
