namespace SunamoFubuCore.Binding;

[Serializable]
public class BindResultAssertionException : Exception
{
    public BindResultAssertionException(Type type, IList<ConvertProblem> problems)
    {
        Type = type;
        Problems = problems;
    }

    protected BindResultAssertionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Type = (Type)info.GetValue("bindType", typeof(Type));
        Problems = (IList<ConvertProblem>)info.GetValue("problems", typeof(IList<ConvertProblem>));
    }

    public override string Message
    {
        get
        {
            var builder = new StringBuilder();
            builder.AppendFormat("Failure while trying to bind object of type '{0}'", Type.FullName);

            Problems.Each(p =>
            {
                builder.AppendFormat("Property: {0}, Value: '{1}', Exception:{2}{3}{2}",
                    p.Property.Name, p.Value, Environment.NewLine, p.ExceptionText);
            });

            return builder.ToString();
        }
    }

    public IList<ConvertProblem> Problems { get; }

    public Type Type { get; }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("bindType", Type);
        info.AddValue("problems", Problems);
    }
}
