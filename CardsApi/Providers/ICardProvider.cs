using CardsApi.Model;

namespace CardsApi.Services;

public interface ICardProvider
{
    Task<CardDetails?> GetCardDetailsAsync(string userId, string cardNumber, CancellationToken cancellationToken);
}