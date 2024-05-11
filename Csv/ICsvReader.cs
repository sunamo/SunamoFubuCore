namespace SunamoFubuCore;

public interface ICsvReader
{
    void Read<T>(CsvRequest<T> request);
}
