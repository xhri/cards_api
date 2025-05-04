namespace CardsApi.Utils;

public class StreamReaderProvider: IStreamReaderProvider
{
    public StreamReader GetStreamReader(string path)
        => new StreamReader(path);
}