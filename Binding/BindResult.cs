namespace SunamoFubuCore;

public class BindResult
{
    public IList<ConvertProblem> Problems = new List<ConvertProblem>();
    public object Value;

    public override string ToString()
    {
        return string.Format("BindResult: {0}, Problems:  {1}", Value, Problems.Count);
    }

    public void AssertNoProblems(Type type)
    {
        if (Problems.Any())
        {
            throw new BindResultAssertionException(type, Problems);
        }
    }

    public void Merge(BindResult result)
    {
        Problems.AddRange(result.Problems);
    }
}
