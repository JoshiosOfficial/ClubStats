namespace ClubStats.AspNetCore.Utilities;

public readonly struct Result<TResult, TError>
{
    public readonly bool IsSuccess { get; }
    public readonly TResult? Value { get; }
    public readonly TError? ErrorValue { get; }

    public Result(TResult result)
    {
        IsSuccess = true;
        Value = result;
        ErrorValue = default;
    }

    public Result(TError error)
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