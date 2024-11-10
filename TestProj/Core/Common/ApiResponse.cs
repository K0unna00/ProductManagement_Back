namespace TestProj.Core.Common;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }

    public ApiResponse()
    {
        IsSuccess = true;
    }

    public ApiResponse(T data)
    {
        Data = data;
        IsSuccess = true;
    }

    public ApiResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
        IsSuccess = false;
    }

    public ApiResponse(string errorMessage, T data)
    {
        ErrorMessage = errorMessage;
        IsSuccess = false;
        Data = data;
    }
}