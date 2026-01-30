namespace ChithraChalanam.Aggregator.Api.Dtos;

public class MovieDetailsResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public decimal? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public string PosterEndpoint { get; set; } = string.Empty;
    public List<MovieStreamResponse> Streams { get; set; } = new();
}

public class MovieStreamResponse
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string PlaybackUrl { get; set; } = string.Empty;
    public string StreamType { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
