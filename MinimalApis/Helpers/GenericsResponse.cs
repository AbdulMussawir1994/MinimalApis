namespace MinimalApis.Helpers;

public class GenericResponse<T>
{
    public T Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Status { get; set; }
    public string Code { get; set; } = string.Empty;

    public static GenericResponse<T> Success(T data, string message = "Success", string code = "200") =>
        new() { Status = true, Message = message, Data = data, Code = code };

    public static GenericResponse<T> Fail(string message = "Failure", string code = "400") =>
        new() { Status = false, Message = message, Code = code };

    public static GenericResponse<T> ExceptionFailed(T data, string message = "Success", string code = "200") =>
        new() { Status = true, Message = message, Data = data, Code = code };
}
