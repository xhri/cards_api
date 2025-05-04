using CardsApi.Model;
using CardsApi.Model.Rules;

namespace CardsApi.Services;

public interface IActionEligibilityService
{
    bool IsActionEligible(CardDetails cardDetails, ActionRules actionRules);
}