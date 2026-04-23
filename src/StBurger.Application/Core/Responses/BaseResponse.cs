namespace StBurger.Application.Core.Responses;

public class BaseResponse
{
    protected BaseResponse()
    {
    }

    public static BaseResponse Ok(string message, int code = 200)
    {
        return new BaseResponse
        {
            Code = code,
            Message = message
        };
    }
    public int Code { get; set; } = 200;
    public string? Message { get; set; } = string.Empty;
}

public class BaseResponse<T> : BaseResponse where T : notnull
{
    public T? Data { get; set; }
    
    public static BaseResponse<T> Ok(string message, int code = 200, T? data = default!)
    {
        return new BaseResponse<T>
        {
            Code = code,
            Message = message,
            Data = data
        };
    }
}
