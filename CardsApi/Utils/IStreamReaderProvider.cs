namespace CardsApi.Utils;

public interface IStreamReaderProvider
{
    StreamReader GetStreamReader(string path);
}