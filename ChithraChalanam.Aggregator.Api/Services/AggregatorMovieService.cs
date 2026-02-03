using ChithraChalanam.Aggregator.Api.Dtos;
using ChithraChalanam.Aggregator.Api.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ChithraChalanam.Aggregator.Api.Services;

public class AggregatorMovieService : IAggregatorMovieService
{
    private readonly IHttpClientFactory httpClientFactory;

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AggregatorMovieService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<AggregatorMovieResponse> GetMovieWithCastAsync(
        int movieId,
        string? accessToken)
    {
        try
        {
            HttpClient movieClient =
                httpClientFactory.CreateClient("MovieService");

            HttpClient castClient =
                httpClientFactory.CreateClient("CastService");

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                movieClient.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue.Parse(accessToken);
            }

            // ---- Movie ----
            HttpResponseMessage movieResponse =
                await movieClient.GetAsync($"/api/movies/{movieId}");

            movieResponse.EnsureSuccessStatusCode();

            string movieJson =
                await movieResponse.Content.ReadAsStringAsync();

            MovieDetailsResponse movie =
                JsonSerializer.Deserialize<MovieDetailsResponse>(
                    movieJson,
                    jsonOptions)!;

            // ---- Cast ----
            HttpResponseMessage castResponse =
                await castClient.GetAsync($"/api/cast/by-movie/{movieId}");

            castResponse.EnsureSuccessStatusCode();

            string castJson =
                await castResponse.Content.ReadAsStringAsync();

            List<MovieCreditDto> cast =
                JsonSerializer.Deserialize<List<MovieCreditDto>>(
                    castJson,
                    jsonOptions) ?? new();

            return new AggregatorMovieResponse
            {
                Movie = movie,
                Cast = cast,
                Links = new AggregatorLinks
                {
                    Movie = $"/api/aggregator/movies/{movieId}",
                    Cast = $"/api/aggregator/movies/{movieId}/cast"
                }
            };
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to aggregate movie details.", ex);
        }
    }

    public async Task<List<MovieCreditDto>> GetCastOnlyAsync(int movieId)
    {
        try
        {
            HttpClient castClient =
                httpClientFactory.CreateClient("CastService");

            HttpResponseMessage response =
                await castClient.GetAsync($"/api/cast/by-movie/{movieId}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<MovieCreditDto>>(
                json,
                jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to fetch cast.", ex);
        }
    }
}
