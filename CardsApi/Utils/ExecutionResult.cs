namespace CardsApi.Utils;

public class ExecutionResult
{
    protected ExecutionResult(bool success, Error? error = null)
    {
        IsSuccess = success;
        Error = error;
    }

    public bool IsSuccess { get; protected set; }
    public Error? Error { get; protected set; }

    public static ExecutionResult Success()
    {
        return new ExecutionResult(true);
    }

    public static ExecutionResult Failure(Error error)
    {
        return new ExecutionResult(false, error);
    }

    public static ExecutionResult Failure(ErrorType errorType, string errorMessage)
        => Failure(new Error(errorType, errorMessage));
    
    public static ExecutionResult<T> Success<T>(T result)
    {
        return new ExecutionResult<T>(result, true);
    }

    public static ExecutionResult<T> Failure<T>(Error error)
    {
        return new ExecutionResult<T>(false, error);
    }

    public static ExecutionResult<T> Failure<T>(ErrorType errorType, string errorMessage)
        => Failure<T>(new Error(errorType, errorMessage));
    
}