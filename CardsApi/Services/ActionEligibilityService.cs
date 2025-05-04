using CardsApi.Model;
using CardsApi.Model.Rules;

namespace CardsApi.Services;

public class ActionEligibilityService : IActionEligibilityService
{
    public bool IsActionEligible(CardDetails cardDetails, ActionRules actionRules)
    {
        if (!actionRules.AllowedCardTypes.Contains(cardDetails.CardType))
            return false;

        var cardStatusRule = actionRules.AllowedCardStatuses.FirstOrDefault(x => x.CardStatus == cardDetails.CardStatus);
        if (cardStatusRule == null)
            return false;

        return cardStatusRule.PinRequirement switch 
        {
            PinRequirement.NoPinRequirement => true,
            PinRequirement.PinSet => cardDetails.IsPinSet,
            PinRequirement.PinNotSet => !cardDetails.IsPinSet,
            _ => false
        };
    }
}