using CardsApi.Model.Rules;
using CardsApi.Utils;

namespace CardsApi.Services;

public interface ICsvActionRulesParser
{
    Task<ExecutionResult<List<ActionRules>>> ImportRulesAsync(string csvFileName, CancellationToken cancellationToken);
}