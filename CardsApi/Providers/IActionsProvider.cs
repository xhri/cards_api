using CardsApi.Model.Dto;
using CardsApi.Utils;

namespace CardsApi.Providers;

public interface IActionsProvider
{
    Task<ExecutionResult<List<ActionDto>>> GetAllowedActionsAsync(string userId, string cardId, CancellationToken cancellationToken); 
}