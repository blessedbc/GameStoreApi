namespace GameStore.Api.Dtos;

public record class GameDetailsDto
(
    int Id,
    string Name,
    int GenreId,
    decimal Price,
    DateTime ReleaseDate
)
{
    public GameDetailsDto(int v1, string v2, int v3, decimal v4, DateOnly dateOnly)
        : this(v1, v2, v3, v4, dateOnly.ToDateTime(TimeOnly.MinValue))
    {
    }
}