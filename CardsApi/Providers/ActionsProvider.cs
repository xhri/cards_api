using CardsApi.Model.Dto;
using CardsApi.Repositories;
using CardsApi.Services;
using CardsApi.Utils;

namespace CardsApi.Providers;

public class ActionsProvider(
    ICardProvider cardProvider,
    IActionRulesRepository actionRulesRepository,
    IActionEligibilityService actionEligibilityService)
    : IActionsProvider
{
    public async Task<ExecutionResult<List<ActionDto>>> GetAllowedActionsAsync(string userId, string cardId, CancellationToken cancellationToken)
    {
        var cardDetails = await cardProvider.GetCardDetailsAsync(userId, cardId, cancellationToken);
        if (cardDetails == null)
        {
            return ExecutionResult.Failure<List<ActionDto>>(ErrorType.NotFound, "Couldn't find the card.");
        }

        var rulesResult = await actionRulesRepository.GetActionRulesAsync(cancellationToken);
        if (!rulesResult.IsSuccess)
        {
            return ExecutionResult.Failure<List<ActionDto>>(rulesResult.Error!);
        }

        var result = new List<ActionDto>();
        foreach (var rule in rulesResult.Result!)
        {
            if (actionEligibilityService.IsActionEligible(cardDetails, rule))
            {
                result.Add(new ActionDto(rule.ActionName));
            }
        }

        return ExecutionResult.Success(result);
    }
}