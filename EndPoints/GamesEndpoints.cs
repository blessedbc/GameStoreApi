using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;

namespace GameStore.Api.EndPoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameSummaryDto> games = [
   new (
    1,
    "Street Fighter II",
    "Fighting",
    19.99m,
    new DateOnly(1992, 7, 15)
),
new (
    2,
    "Final Fantasy XIV",
    "Roleplaying",
    59.99m,
    new DateOnly(2010, 9, 30)
),
new (
    3,
    "FIFA 23",
    "Sports",
    69.99m,
    new DateOnly(2022, 9, 27)
)
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                       .WithParameterValidation();

       // Get all games
        group.MapGet("/", () => games);

  // Get game by ID
  group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
 {
    Game? game = dbContext.Games.Find(id);

    return game is not null ? Results.Ok(game.ToGameDetailsDto()) : Results.NotFound();
 })
 .WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            dbContext.SaveChanges(); 
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
        });

   // PUT /games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
    {
    var index = games.FindIndex(game => game.Id == id);

     if (index == -1)
     {
        return Results.NotFound();
     }
    
     games[index] = new GameSummaryDto(
         id,
         updatedGame.Name,
         updatedGame.Genre,
         updatedGame.Price,
         updatedGame.ReleaseDate
     );
     return Results.NoContent();
     });

      // DELETE /games/{id}
       group.MapDelete("/{id}", (int id) =>
      {
      games.RemoveAll(game => game.Id == id);
      return Results.NoContent();
     });
       return group;
    }

}    