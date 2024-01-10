namespace SunamoFubuCore;

public static class NumberExtensions
{
    [DebuggerStepThrough]
    public static int Times(this int maxCount, Action<int> eachAction)
    {
        for (var idx = 0; idx < maxCount; idx++) eachAction(idx);

        return maxCount;
    }
}
