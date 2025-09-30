namespace LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public ApiResponse(T data, string message = "Operación exitosa")
    {
        Success = true;
        Message = message;
        Data = data;
    }

    public ApiResponse(string message)
    {
        Success = false;
        Message = message;
    }
}
