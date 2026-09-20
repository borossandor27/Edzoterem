namespace Edzoterem.Application.Common;

public class ApiException : Exception
{
    public int StatusCode { get; }

    public ApiException(string message, int statusCode = 400) : base(message)
    {
        StatusCode = statusCode;
    }

    public static ApiException NotFound(string message) => new(message, 404);
    public static ApiException Conflict(string message) => new(message, 409);
    public static ApiException BadRequest(string message) => new(message, 400);
}
