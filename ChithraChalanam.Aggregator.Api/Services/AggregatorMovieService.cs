using ChithraChalanam.Aggregator.Api.Common;
using ChithraChalanam.Aggregator.Api.Dtos;
using ChithraChalanam.Aggregator.Api.Helper;
using ChithraChalanam.Aggregator.Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ChithraChalanam.Aggregator.Api.Services;

public class AggregatorMovieService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache) : IAggregatorMovieService
{
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
    private readonly IMemoryCache memoryCache = memoryCache;
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<ApiResponse<AggregatorMovieResponse>> GetMovieWithCastAsync(int movieId)
    {
        string cacheKey = $"movie-aggregate-{movieId}";

        if (memoryCache.TryGetValue(cacheKey, out AggregatorMovieResponse cached))
        {
            return ApiResponseHelper.Success(
                cached,
                "Fetched from cache");
        }

        HttpClient movieClient =
            httpClientFactory.CreateClient("MovieService");

        HttpClient castClient =
            httpClientFactory.CreateClient("CastService");

        HttpClient streamClient =
            httpClientFactory.CreateClient("StreamService");

        Task<HttpResponseMessage> movieTask =
            movieClient.GetAsync($"/api/movies/{movieId}");

        Task<HttpResponseMessage> castTask =
            castClient.GetAsync($"/api/cast/movie/{movieId}");

        Task<HttpResponseMessage> streamTask =
            streamClient.GetAsync($"/api/stream/online?movieId={movieId}");

        HttpResponseMessage movieResponse = await movieTask;

        if (!movieResponse.IsSuccessStatusCode)
        {
            return ApiResponseHelper.Fail<AggregatorMovieResponse>(
                "Failed to fetch movie details",
                (int)movieResponse.StatusCode);
        }

        string movieJson =
            await movieResponse.Content.ReadAsStringAsync();

        MovieDetailsResponse movie =
            JsonSerializer.Deserialize<MovieDetailsResponse>(movieJson, jsonOptions)!;

        List<MovieCreditDto> cast = [];
        List<AggregatorStreamingDto> stream = [];

        try
        {
            HttpResponseMessage castResponse = await castTask;

            if (castResponse.IsSuccessStatusCode)
            {
                string castJson =
                    await castResponse.Content.ReadAsStringAsync();

                cast =
                    JsonSerializer.Deserialize<List<MovieCreditDto>>(castJson, jsonOptions) ?? [];
            }
        }
        catch { }

        try
        {
            HttpResponseMessage streamResponse = await streamTask;

            if (streamResponse.IsSuccessStatusCode)
            {
                string streamJson =
                    await streamResponse.Content.ReadAsStringAsync();

                stream =
                    JsonSerializer.Deserialize<List<AggregatorStreamingDto>>(streamJson, jsonOptions) ?? [];
            }
        }
        catch { }

        var result = new AggregatorMovieResponse
        {
            Movie = movie,
            Cast = cast,
            Streaming = stream,
            Links = new AggregatorLinks
            {
                Movie = $"/api/aggregator/movies/{movieId}",
                Cast = $"/api/aggregator/movies/{movieId}/cast",
                Streaming = $"/api/aggregator/movies/{movieId}/streaming"
            }
        };

        memoryCache.Set(
    cacheKey,
    result,
    TimeSpan.FromMinutes(5));

        return ApiResponseHelper.Success(
            result,
            "Movie with cast and stream links fetched successfully");

    }



    public async Task<ApiResponse<List<MovieCreditDto>>> GetCastOnlyAsync(int movieId)
    {
        HttpClient castClient =
            httpClientFactory.CreateClient("CastService");

        HttpResponseMessage response =
            await castClient.GetAsync($"/api/cast/movie/{movieId}");

        if (!response.IsSuccessStatusCode)
        {
            return ApiResponseHelper.Fail<List<MovieCreditDto>>(
                "Failed to fetch cast",
                (int)response.StatusCode);
        }

        string json =
            await response.Content.ReadAsStringAsync();

        List<MovieCreditDto> cast =
            JsonSerializer.Deserialize<List<MovieCreditDto>>(json, jsonOptions) ?? [];

        return ApiResponseHelper.Success(
            cast,
            "Cast fetched successfully");
    }

}

