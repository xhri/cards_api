namespace CardsApi.Model.Rules;

public record ActionRules(string ActionName, List<CardType> AllowedCardTypes, List<CardStatusRule> AllowedCardStatuses);