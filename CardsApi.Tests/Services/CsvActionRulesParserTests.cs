using System.Text;
using CardsApi.Model;
using CardsApi.Services;
using CardsApi.Utils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CardsApi.Tests.Services;

public class CsvActionRulesParserTests
{
    private readonly Mock<IStreamReaderProvider> _streamReaderProviderMock = new();
    private readonly Mock<ILogger<CsvActionRulesParser>> _loggerMock = new();

    [Fact]
    public async Task ImportRulesAsync_ShouldReturnCorrectValue_WithSingleRow()
    {
        // Arrange
        var sut = new CsvActionRulesParser(_loggerMock.Object, _streamReaderProviderMock.Object);
        var csvContent = @"Action,Prepaid,Debit,Credit,Ordered,Inactive,Active,Restricted,Blocked,Expired,Closed
ACTION1,YES,YES,YES,NO,NO,YES,NO,NO,NO,NO";
        MockCsvContent(csvContent);

        // Act
        var result = await sut.ImportRulesAsync("fileName", default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result.Count.Should().Be(1);
        result.Result[0].ActionName.Should().Be("ACTION1");
        result.Result[0].AllowedCardStatuses.Count.Should().Be(1);
        result.Result[0].AllowedCardStatuses[0].CardStatus.Should().Be(CardStatus.Active);
    }

    [Fact]
    public async Task ImportRulesAsync_ShouldReturnCorrectValue_WithManyRows()
    {
        // Arrange
        var sut = new CsvActionRulesParser(_loggerMock.Object, _streamReaderProviderMock.Object);
        var csvContent = @"Action,Prepaid,Debit,Credit,Ordered,Inactive,Active,Restricted,Blocked,Expired,Closed
ACTION1,YES,YES,YES,NO,NO,YES,NO,NO,NO,NO
ACTION2,YES,YES,YES,NO,NO,YES,NO,NO,NO,NO
ACTION3,NO,NO,YES,YES,NO,YES,NO,NO,NO,NO";
        MockCsvContent(csvContent);

        // Act
        var result = await sut.ImportRulesAsync("fileName", default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result.Count.Should().Be(3);
        result.Result[0].ActionName.Should().Be("ACTION1");
        result.Result[0].AllowedCardStatuses.Count.Should().Be(1);
        result.Result[0].AllowedCardStatuses[0].CardStatus.Should().Be(CardStatus.Active);
        result.Result[2].ActionName.Should().Be("ACTION3");
        result.Result[2].AllowedCardStatuses.Count.Should().Be(2);
        result.Result[2].AllowedCardTypes.Count.Should().Be(1);
    }

    private void MockCsvContent(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);
        var reader = new StreamReader(stream);

        _streamReaderProviderMock
            .Setup(mock => mock.GetStreamReader(It.IsAny<string>()))
            .Returns(reader);
    }

}