using CardsApi.Repositories;

namespace CardsApi.Services;

public class CsvImporter(ICsvActionRulesParser csvActionRulesParser, IActionRulesRepository actionRulesRepository) : ICsvImporter
{
    public async Task ImportCsvAsync(string csvFileName, CancellationToken cancellationToken)
    {
        var result = await csvActionRulesParser.ImportRulesAsync(csvFileName, cancellationToken);
        if (result.IsSuccess){
            foreach(var rule in result.Result!)
                await actionRulesRepository.UpdateActionRulesAsync(rule, cancellationToken);
        }
    }
}