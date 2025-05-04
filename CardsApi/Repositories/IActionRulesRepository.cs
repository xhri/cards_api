using CardsApi.Model.Rules;
using CardsApi.Utils;

namespace CardsApi.Repositories;

public interface IActionRulesRepository
{
    Task<ExecutionResult<List<ActionRules>>> GetActionRulesAsync(CancellationToken cancellationToken);
    Task<ExecutionResult> UpdateActionRulesAsync(ActionRules actionRules, CancellationToken cancellationToken);
}