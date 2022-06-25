using Microsoft.AspNetCore.Mvc;

namespace ClubStats.AspNetCore.Utilities;

public record ApiError(int Code, string Message)
{
    public IActionResult Result()
    {
        if (String.IsNullOrEmpty(Message))
        {
            return new StatusCodeResult(Code);
        }

        return new ObjectResult(this) { StatusCode = Code };
    }
}

public readonly struct Result<TResult, TError>
{
    private readonly bool IsSuccess { get; }
    private readonly TResult? Value { get; }
    private readonly TError? ErrorValue { get; }

    private Result(TResult result)
    {
        IsSuccess = true;
        Value = result;
        ErrorValue = default;
    }

    private Result(TError error)
    {
        IsSuccess = false;
        Value = default;
        ErrorValue = error;
    }

    public static Result<TResult, TError> Ok(TResult value)
    {
        return new Result<TResult, TError>(value);
    }

    public static Result<TResult, TError> Error(TError error)
    {
        return new Result<TResult, TError>(error);
    }

    public Result<T, TError> Bind<T>(Func<TResult, Result<T, TError>> func)
    {
        return IsSuccess ? func(Value!) : new Result<T, TError>(ErrorValue!);
    }

    public void Match(Action<TResult> onResult, Action<TError> onError)
    {
        if (IsSuccess) onResult(Value!);
        else onError(ErrorValue!);
    }
    
    public T Match<T>(Func<TResult, T> onResult, Func<TError, T> onError)
    {
        return IsSuccess ? onResult(Value!) : onError(ErrorValue!);
    }
}