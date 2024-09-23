public class Result
{
    public bool Success { get; }
    public string? ErrorMessage { get; }

    protected Result(bool success, string? errorMessage)
    {
        Success = success;
        ErrorMessage = errorMessage;
    }

    public static Result Ok() => new Result(true, null);
    public static Result Fail(string errorMessage) => new Result(false, errorMessage);
}

public class Result<T> : Result
{
    public T Value { get; }

    private Result(bool success, T value, string? errorMessage)
        : base(success, errorMessage)
    {
        Value = value;
    }

    public static Result<T> Ok(T value) => new Result<T>(true, value, null);
    public static Result<T> Fail(string errorMessage) => new Result<T>(false, default!, errorMessage);
}
