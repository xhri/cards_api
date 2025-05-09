using AutoMapper;
using CardsApi.Model.Requests;
using CardsApi.Model.Response;
using CardsApi.Providers;
using CardsApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CardsApi.Controllers;

[ApiController]
[Route("users/{userId}/cards/{cardId}/actions")]
public class CardActionsController(
    ILogger<CardActionsController> logger,
    IActionsProvider actionsProvider,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CardActionsRequest request, CancellationToken cancellationToken)
    {
        var result = await actionsProvider.GetAllowedActionsAsync(request.UserId, request.CardId, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(mapper.Map<List<ActionResponse>>(result.Result));
        }
        else
        {
            logger.LogWarning($"Error from action provider: {result.Error!.errorMessage}");
            return result.Error!.errorType switch
            {
                ErrorType.NotFound => NotFound(result.Error.errorMessage),
                _ => BadRequest(result.Error.errorMessage),
            };
        }
    }
}
