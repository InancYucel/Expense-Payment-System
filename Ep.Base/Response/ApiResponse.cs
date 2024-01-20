using System.Text.Json;

namespace Base.Response;

public class ApiResponse  //All Responses are derived from this class
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    // Our aim is to prepare the response of all classes through a single class.
    public ApiResponse(string message = null)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Success = true;
        }
        else
        {
            Success = false;
            Message = message;
        }
    }
    public bool Success { get; set; }
    public string Message { get; set; }
    public DateTime ServerDate { get; set; } = DateTime.UtcNow;
    public Guid ReferenceNo { get; set; } = Guid.NewGuid();
}

public class ApiResponse<T>
{
    public DateTime ServerDate { get; set; } = DateTime.UtcNow;
    public Guid ReferenceNo { get; set; } = Guid.NewGuid();
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Response { get; set; }

    public ApiResponse(bool isSuccess)
    {
        Success = isSuccess;
        Response = default;
        Message = isSuccess ? "Success" : "Error";
    }
    public ApiResponse(T data)
    {
        Success = true;
        Response = data;
        Message = "Success";
    }
    public ApiResponse(string message)
    {
        Success = false;
        Response = default;
        Message = message;
    }
}