using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.EndPoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                       .WithParameterValidation();

       // Get all games
        group.MapGet("/", (GameStoreContext dbContext) =>
        dbContext.Games
        .Include(game => game.Genre)
        .Select(game => game.ToGameSummaryDto())
        .AsNoTracking());

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
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
    {
    var existingGame = dbContext.Games.Find(id);

     if (existingGame is null)
     {
        return Results.NotFound();
     }

        dbContext.Entry(existingGame)
        .CurrentValues
        .SetValues(updatedGame.ToEntity(id));
        dbContext.SaveChanges();

     return Results.NoContent();
     });

      // DELETE /games/{id}
       group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
      {
          dbContext.Games
          .Where(game => game.Id == id)
          .ExecuteDelete();

      return Results.NoContent();
     });
       return group;
    }

}    