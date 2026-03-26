namespace DocDes.Core.Responses;

public class ErrorResponse : Response
{
    public string ExceptionMessage { get; }

    public ErrorResponse(string message) : base(false, message)
    {
        ExceptionMessage = "General Exception";
    }
    public ErrorResponse(string exceptionMessage, string message) : base(false, message)
    {
        ExceptionMessage = exceptionMessage;
    }

    public ErrorResponse() : base(false, "Failed with errors")
    {
        ExceptionMessage = "General Exception";
    }
}