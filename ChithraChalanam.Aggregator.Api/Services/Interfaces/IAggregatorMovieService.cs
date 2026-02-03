using ChithraChalanam.Aggregator.Api.Dtos;

namespace ChithraChalanam.Aggregator.Api.Services.Interfaces;

public interface IAggregatorMovieService
{
    Task<AggregatorMovieResponse> GetMovieWithCastAsync(
        int movieId,
        string? accessToken);

    Task<List<MovieCreditDto>> GetCastOnlyAsync(int movieId);
}
