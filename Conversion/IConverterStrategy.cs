namespace FubuCore.Conversion
{
    public interface IConverterStrategy : DescribesItself
    {
        object Convert(IConversionRequest request);
    }
}
