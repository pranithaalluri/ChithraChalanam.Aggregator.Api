using ChithraChalanam.Aggregator.Api.Common;
using ChithraChalanam.Aggregator.Api.Dtos;

namespace ChithraChalanam.Aggregator.Api.Services.Interfaces;

public interface IAggregatorMovieService
{
    Task<ApiResponse<AggregatorMovieResponse>> GetMovieWithCastAsync(
     int movieId);

    Task<ApiResponse<List<MovieCreditDto>>> GetCastOnlyAsync(int movieId);
}
