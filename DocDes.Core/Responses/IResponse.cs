namespace DocDes.Core.Responses;

public interface IResponse
{
    bool Success { get; }
    string Message { get; }
}