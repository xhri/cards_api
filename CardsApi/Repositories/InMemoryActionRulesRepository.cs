using CardsApi.Model.Rules;
using CardsApi.Utils;

namespace CardsApi.Repositories;
public class InMemoryActionRulesRepository : IActionRulesRepository
{
    private List<ActionRules> _rules = [];

    public Task<ExecutionResult<List<ActionRules>>> GetActionRulesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(ExecutionResult.Success(_rules));
    }

    public Task<ExecutionResult> UpdateActionRulesAsync(ActionRules actionRules, CancellationToken cancellationToken)
    {
        _rules.RemoveAll(rule => rule.ActionName == actionRules.ActionName);
        _rules.Add(actionRules);

        return Task.FromResult(ExecutionResult.Success());
    }
}