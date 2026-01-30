namespace ChithraChalanam.Aggregator.Api.Dtos;


public class MovieCreditDto
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int PersonId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? CharacterName { get; set; }
    public int Order { get; set; }
    public PersonDto? Person { get; set; }
}

public class PersonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public string? YoutubeUrl { get; set; }
}