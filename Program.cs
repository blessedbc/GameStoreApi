using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStore.Api.Data.GameStoreContext>(connString);

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
