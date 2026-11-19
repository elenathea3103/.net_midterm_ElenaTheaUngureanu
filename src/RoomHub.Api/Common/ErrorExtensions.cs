namespace RoomHub.Api.Common;

public static class ErrorExtensions
{
    public static IResult ToProblem(this Error error) => Results.Problem(
        title: error.Code,
        detail: error.Message,
        statusCode: error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        });
}
