namespace DatingTelegramBot.Domain.Errors;

public sealed class Result<TValue, TError>
{
    public readonly TValue? _value;
    public readonly TError? _error;

    private bool _isSuccess;

    private Result(TValue value)
    {
        _isSuccess = true;
        _value = value;
        _error = default;
    }

    private Result(TError error)
    {
        _isSuccess = false;
        _value = default;
        _error = error;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);

    public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);

    public Result<TValue, TError> Match(Func<TValue, Result<TValue, TError>> success, Func<TError, Result<TValue, TError>> failure)
    {
        if (_isSuccess)
        {
            return success(_value!);
        }
        return failure(_error!);
    }

}
