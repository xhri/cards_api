using System.Globalization;
using CardsApi.Model;
using CardsApi.Model.Rules;
using CardsApi.Utils;
using CsvHelper;

namespace CardsApi.Services;

public class CsvActionRulesParser(ILogger<CsvActionRulesParser> logger, IStreamReaderProvider streamReaderProvider) : ICsvActionRulesParser
{
    private const string ActionNameHeader = "Action";
    private const string YesValue = "YES";
    private const string PinSetNecessary = "PIN";
    private const string NoPinSetNecessary = "NOPIN";

    public async Task<ExecutionResult<List<ActionRules>>> ImportRulesAsync(string csvFileName, CancellationToken cancellationToken)
    {
        try
        {
            var results = new List<ActionRules>();
            using var reader = streamReaderProvider.GetStreamReader(csvFileName);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var rows = csv.GetRecordsAsync<dynamic>(cancellationToken);
            await foreach (var row in rows)
            {
                results.Add(ParseRow((IDictionary<string, object>)row));
            }

            return ExecutionResult.Success(results);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Couldn't parse CSV file");
            return ExecutionResult.Failure<List<ActionRules>>(ErrorType.GeneralError, "Couldn't parse CSV file");
        }
    }

    private ActionRules ParseRow(IDictionary<string, object> row)
    {
        var result = new ActionRules(row[ActionNameHeader].ToString()!, [], []);
        foreach (var cardType in Enum.GetValues<CardType>())
        {
            if (row.TryGetValue(cardType.ToString(), out var value) && value.ToString() == YesValue)
            {
                result.AllowedCardTypes.Add(cardType);
            }
        }

        foreach (var cardStatus in Enum.GetValues<CardStatus>())
        {
            if (row.TryGetValue(cardStatus.ToString(), out var value))
            {
                PinRequirement? pinRequirement = value switch
                {
                    YesValue => PinRequirement.NoPinRequirement,
                    PinSetNecessary => PinRequirement.PinSet,
                    NoPinSetNecessary => PinRequirement.PinNotSet,
                    _ => null
                };

                if (pinRequirement != null)
                    result.AllowedCardStatuses.Add(new CardStatusRule(cardStatus, pinRequirement.Value));
            }
        }

        return result;
    }
}