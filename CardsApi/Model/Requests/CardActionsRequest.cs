using Microsoft.AspNetCore.Mvc;

namespace CardsApi.Model.Requests;

public class CardActionsRequest
{
    [FromRoute(Name = "userId")]
    public required string UserId { get; set; }

    [FromRoute(Name = "cardId")]
    public required string CardId { get; set; }
}