namespace ClubStats.AspNetCore.Utilities;

public record ApiError(int Code, string Message);

public readonly struct Option<T>
{
    public readonly bool HasValue { get; }
    public readonly T? Value { get; }

    public Option()
    {
        HasValue = false;
        Value = default;
    }
    
    public Option(T value)
    {
        HasValue = true;
        Value = value;
    }

    public static Option<T> Some(T value)
    {
        return new Option<T>(value);
    }

    public static Option<T> None()
    {
        return new Option<T>();
    }

    public Option<TNew> Bind<TNew>(Func<T, Option<TNew>> func)
    {
        return HasValue ? func(Value!) : new Option<TNew>();
    }

    public void Match(Action<T> onValue, Action onNone)
    {
        if (HasValue) onValue(Value!);
        else onNone();
    }
    
    public T1 Match<T1>(Func<T, T1> onValue, Func<T1> onNone)
    {
        return HasValue ? onValue(Value!) : onNone();
    }
}