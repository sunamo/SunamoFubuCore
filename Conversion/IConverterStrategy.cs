namespace SunamoFubuCore;

public interface IConverterStrategy : DescribesItself
{
    object Convert(IConversionRequest request);
}
