namespace GameStore.Api.Dtos;

public record class GameSummaryDto
(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDat)
{
    public GameSummaryDto(int v1, string v2, string v3, decimal v4, DateOnly dateOnly)
        : this(v1, v2, v3, v4, dateOnly.ToDateTime(TimeOnly.MinValue))
    {
    }
}