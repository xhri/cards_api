namespace CardsApi.Utils;

public class ExecutionResult<T> : ExecutionResult
{
    internal ExecutionResult(T result, bool success, Error? error = null)
     : base(success, error)
    {
        Result = result;
    }

    internal ExecutionResult(bool success, Error? error = null)
     : base(success, error)
    {
    }

    public T? Result { get; private set;}
}