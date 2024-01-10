namespace SunamoFubuCore.Csv;

public interface ICsvReader
{
    void Read<T>(CsvRequest<T> request);
}
