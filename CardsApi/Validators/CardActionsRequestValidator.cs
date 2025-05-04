using CardsApi.Model.Requests;
using FluentValidation;

namespace CardsApi.Validators;

public class CardActionsRequestValidator : AbstractValidator<CardActionsRequest>
{
    public CardActionsRequestValidator()
    {
        RuleFor(x => x.CardId)
            .NotEmpty()
            .WithMessage("CardId cannot be empty.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty.");
    }
}