using CardsApi.Services;
using CardsApi.Model;
using CardsApi.Model.Rules;
using FluentAssertions;

namespace CardsApi.Tests.Services;

public class ActionEligibilityServiceTests
{
    [Fact]
    public void IsActionEligible_ShouldReturnTrue_ExactlyMeetsRequirements()
    {
        // Arrange
        var sut = new ActionEligibilityService();
        var cardDetails = new CardDetails("num1", CardType.Debit, CardStatus.Inactive, true);
        var actionRules = new ActionRules("actionName", [CardType.Debit], [new (CardStatus.Inactive, PinRequirement.NoPinRequirement)]);

        // Act
        var result = sut.IsActionEligible(cardDetails, actionRules);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsActionEligible_ShouldReturnTrue_OneOfManyRequirementsMet()
    {
        // Arrange
        var sut = new ActionEligibilityService();
        var cardDetails = new CardDetails("num1", CardType.Debit, CardStatus.Inactive, true);
        var actionRules = new ActionRules(
            "actionName",
            [CardType.Debit, CardType.Credit],
            [
                new (CardStatus.Inactive, PinRequirement.NoPinRequirement),
                new (CardStatus.Active, PinRequirement.NoPinRequirement),
                new (CardStatus.Closed, PinRequirement.PinSet),
                new (CardStatus.Blocked, PinRequirement.NoPinRequirement)]);

        // Act
        var result = sut.IsActionEligible(cardDetails, actionRules);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsActionEligible_ShouldReturnTrue_PinRequirementMet()
    {
        // Arrange
        var sut = new ActionEligibilityService();
        var cardDetails = new CardDetails("num1", CardType.Debit, CardStatus.Inactive, true);
        var actionRules = new ActionRules(
            "actionName",
            [CardType.Debit, CardType.Credit],
            [
                new (CardStatus.Inactive, PinRequirement.PinSet),
                new (CardStatus.Blocked, PinRequirement.NoPinRequirement)]);

        // Act
        var result = sut.IsActionEligible(cardDetails, actionRules);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsActionEligible_ShouldReturnFalse_PinRequirementNotMet()
    {
        // Arrange
        var sut = new ActionEligibilityService();
        var cardDetails = new CardDetails("num1", CardType.Debit, CardStatus.Inactive, true);
        var actionRules = new ActionRules(
            "actionName",
            [CardType.Debit, CardType.Credit],
            [
                new (CardStatus.Inactive, PinRequirement.PinNotSet),
                new (CardStatus.Blocked, PinRequirement.NoPinRequirement)]);

        // Act
        var result = sut.IsActionEligible(cardDetails, actionRules);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsActionEligible_ShouldReturnFalse_CardStatusConditionMetButTypeNot()
    {
        // Arrange
        var sut = new ActionEligibilityService();
        var cardDetails = new CardDetails("num1", CardType.Debit, CardStatus.Inactive, true);
        var actionRules = new ActionRules(
            "actionName",
            [CardType.Prepaid, CardType.Credit],
            [
                new (CardStatus.Inactive, PinRequirement.NoPinRequirement),
                new (CardStatus.Blocked, PinRequirement.NoPinRequirement)]);

        // Act
        var result = sut.IsActionEligible(cardDetails, actionRules);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsActionEligible_ShouldReturnFalse_CardTypeConditionMetButCardStatusNot()
    {
        // Arrange
        var sut = new ActionEligibilityService();
        var cardDetails = new CardDetails("num1", CardType.Debit, CardStatus.Inactive, true);
        var actionRules = new ActionRules(
            "actionName",
            [CardType.Debit, CardType.Credit],
            [
                new (CardStatus.Blocked, PinRequirement.NoPinRequirement)]);

        // Act
        var result = sut.IsActionEligible(cardDetails, actionRules);

        // Assert
        result.Should().BeFalse();
    }
}