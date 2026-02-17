namespace ChithraChalanam.Aggregator.Api.Dtos;

public class AggregatorMovieResponse
{
    private static readonly List<MovieCreditDto> movieCreditDtos = new();

    public MovieDetailsResponse? Movie { get; set; }
    public List<MovieCreditDto> Cast { get; set; } = movieCreditDtos;
    public List<AggregatorStreamingDto> Streaming { get; set; } = [];
    public AggregatorLinks Links { get; set; } = new();
}

public class AggregatorLinks
{
    public string Movie { get; set; } = string.Empty;
    public string Cast { get; set; } = string.Empty;
    public string Streaming { get; set; }= string.Empty;
}
