using CardsApi.Model;
using CardsApi.Model.Rules;
using CardsApi.Providers;
using CardsApi.Repositories;
using CardsApi.Services;
using CardsApi.Utils;
using FluentAssertions;
using Moq;

namespace CardsApi.Tests.Providers;

public class ActionsProviderTests
{
    private readonly Mock<ICardProvider> _cardProviderMock = new();
    private readonly Mock<IActionRulesRepository> _actionRulesRepository = new();
    private readonly Mock<IActionEligibilityService> _actionEligibilityService = new();

    private const string _userId = "user1";
    private const string _cardId = "card11";

    [Fact]
    public async Task GetAllowedActionsAsync_ShouldReturnCorrectResult_BasicHappyPath()
    {
        // Arrange
        var sut = new ActionsProvider(_cardProviderMock.Object, _actionRulesRepository.Object, _actionEligibilityService.Object);
        var cardDetails = new CardDetails(_cardId, CardType.Debit, CardStatus.Active, true);
        var actionName = "action1";
        var actionRules = new ActionRules(actionName, [], []);
        _cardProviderMock
            .Setup(mock => mock.GetCardDetailsAsync(_userId, _cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cardDetails);
        _actionRulesRepository
            .Setup(mock => mock.GetActionRulesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(ExecutionResult.Success<List<ActionRules>>([actionRules]));
        _actionEligibilityService
            .Setup(mock => mock.IsActionEligible(cardDetails, actionRules))
            .Returns(true);

        // Act
        var result = await sut.GetAllowedActionsAsync(_userId, _cardId, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result.Count.Should().Be(1);
        result.Result[0].Name.Should().Be(actionName);
    }

    [Fact]
    public async Task GetAllowedActionsAsync_ShouldReturnCorrectResults_WithMultipleActions()
    {
        // Arrange
        var sut = new ActionsProvider(_cardProviderMock.Object, _actionRulesRepository.Object, _actionEligibilityService.Object);
        var cardDetails = new CardDetails(_cardId, CardType.Debit, CardStatus.Active, true);
        var actionName = "action1";
        var actionRules = new ActionRules(actionName, [], []);
        var actionName2 = "action1";
        var actionRules2 = new ActionRules(actionName2, [], []);
        _cardProviderMock
            .Setup(mock => mock.GetCardDetailsAsync(_userId, _cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cardDetails);
        _actionRulesRepository
            .Setup(mock => mock.GetActionRulesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(ExecutionResult.Success<List<ActionRules>>([actionRules, actionRules2]));
        _actionEligibilityService
            .Setup(mock => mock.IsActionEligible(cardDetails, actionRules))
            .Returns(true);
        _actionEligibilityService
            .Setup(mock => mock.IsActionEligible(cardDetails, actionRules2))
            .Returns(true);

        // Act
        var result = await sut.GetAllowedActionsAsync(_userId, _cardId, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetAllowedActionsAsync_ShouldReturnEmptyList_NothingMatches()
    {
        // Arrange
        var sut = new ActionsProvider(_cardProviderMock.Object, _actionRulesRepository.Object, _actionEligibilityService.Object);
        var cardDetails = new CardDetails(_cardId, CardType.Debit, CardStatus.Active, true);
        var actionName = "action1";
        var actionRules = new ActionRules(actionName, [], []);
        var actionName2 = "action1";
        var actionRules2 = new ActionRules(actionName2, [], []);
        _cardProviderMock
            .Setup(mock => mock.GetCardDetailsAsync(_userId, _cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cardDetails);
        _actionRulesRepository
            .Setup(mock => mock.GetActionRulesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(ExecutionResult.Success<List<ActionRules>>([actionRules, actionRules2]));
        _actionEligibilityService
            .Setup(mock => mock.IsActionEligible(cardDetails, actionRules))
            .Returns(false);
        _actionEligibilityService
            .Setup(mock => mock.IsActionEligible(cardDetails, actionRules2))
            .Returns(false);

        // Act
        var result = await sut.GetAllowedActionsAsync(_userId, _cardId, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result.Count.Should().Be(0);
    }

    [Fact]
    public async Task GetAllowedActionsAsync_ShouldReturnFailResult_CardNotFound()
    {
        // Arrange
        var sut = new ActionsProvider(_cardProviderMock.Object, _actionRulesRepository.Object, _actionEligibilityService.Object);

        // Act
        var result = await sut.GetAllowedActionsAsync(_userId, _cardId, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.errorType.Should().Be(ErrorType.NotFound);
    }
}