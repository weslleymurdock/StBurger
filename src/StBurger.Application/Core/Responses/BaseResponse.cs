namespace StBurger.Application.Core.Responses;

public class BaseResponse
{
    public int Code { get; set; } = 200;
    public string? Message { get; set; } = string.Empty;
}

public class BaseResponse<T>(T data) : BaseResponse where T : class
{
    public T? Data { get; set; } = data;
}
