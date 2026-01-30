namespace ChithraChalanam.Aggregator.Api.Dtos;

public class AggregatorMovieResponse
{
    public MovieDetailsResponse? Movie { get; set; }
    public List<MovieCreditDto> Cast { get; set; } = new();
    public AggregatorLinks Links { get; set; } = new();
}

public class AggregatorLinks
{
    public string Movie { get; set; } = string.Empty;
    public string Cast { get; set; } = string.Empty;
}
