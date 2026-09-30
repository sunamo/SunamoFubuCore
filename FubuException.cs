namespace SunamoFubuCore;

[Serializable]
public class FubuException : Exception
{
    private readonly string _message;

    public FubuException(int errorCode, string message)
    : base(message)
    {
        ErrorCode = errorCode;
        _message = message;
    }

    private FubuException(int errorCode, string message, Exception innerException)
    : base(message, innerException)
    {
        ErrorCode = errorCode;
        _message = message;
    }

    protected FubuException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
        ErrorCode = info.GetInt32("errorCode");
        _message = info.GetString("message");
    }

    public FubuException(int errorCode, Exception inner, string template, params string[] substitutions)
    : this(errorCode, template.ToFormat(substitutions), inner)
    {
    }

    public FubuException(int errorCode, string template, params string[] substitutions)
    : this(errorCode, template.ToFormat(substitutions))
    {
    }

    public override string Message => "FubuCore Error {0}:  \n{1}".ToFormat(ErrorCode, _message);

    public int ErrorCode { get; }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("errorCode", ErrorCode);
        info.AddValue("message", _message);
    }
}


[Serializable]
public class FubuAssertionException : Exception
{
    public FubuAssertionException(string message) : base(message)
    {
    }

    protected FubuAssertionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
