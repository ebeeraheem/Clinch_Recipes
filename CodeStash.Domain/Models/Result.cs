using System.Text.Json.Serialization;

namespace CodeStash.Domain.Models;
public class Result<T> : Result
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T Value { get; }

    [JsonConstructor]
    private Result(bool isSuccess, T value, Error error) : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, null!);

    public static Result<T> Failure(Error error, bool isSuccess = false) =>
        new(isSuccess, default!, error);
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Error Error { get; }

    [JsonConstructor]
    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null!);
    public static Result Failure(Error error) => new(false, error);
}
