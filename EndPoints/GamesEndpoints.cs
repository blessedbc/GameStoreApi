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
        group.MapGet("/", async (GameStoreContext dbContext) =>
     await  dbContext.Games
        .Include(game => game.Genre)
        .Select(game => game.ToGameSummaryDto())
        .AsNoTracking()
        .ToListAsync());

  // Get game by ID
  group.MapGet("/{id}",  async (int id, GameStoreContext dbContext) =>
 {
    Game? game = await dbContext.Games.FindAsync(id);

    return game is not null ? Results.Ok(game.ToGameDetailsDto()) : Results.NotFound();
 })
 .WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync(); 
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
        });

   // PUT /games
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
    {
    var existingGame = await dbContext.Games.FindAsync(id);

     if (existingGame is null)
     {
        return Results.NotFound();
     }

        dbContext.Entry(existingGame)
        .CurrentValues
        .SetValues(updatedGame.ToEntity(id));
        await dbContext.SaveChangesAsync();

     return Results.NoContent();
     });

      // DELETE /games/{id}
       group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
      {
          await dbContext.Games
          .Where(game => game.Id == id)
          .ExecuteDeleteAsync();

      return Results.NoContent();
     });
       return group;
    }

}    