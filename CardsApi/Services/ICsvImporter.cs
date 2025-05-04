namespace CardsApi.Services;

public interface ICsvImporter
{
    Task ImportCsvAsync(string csvFileName, CancellationToken cancellationToken);
}