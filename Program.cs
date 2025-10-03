using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
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
// Gtet all games
app.MapGet("games", () => games);

// Get game by ID
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id))
.WithName(GetGameEndpointName);

//POST /games
app.MapPost("games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

app.Run();
