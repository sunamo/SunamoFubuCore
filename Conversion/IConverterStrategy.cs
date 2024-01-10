namespace SunamoFubuCore.Conversion;

public interface IConverterStrategy : DescribesItself
{
    object Convert(IConversionRequest request);
}
