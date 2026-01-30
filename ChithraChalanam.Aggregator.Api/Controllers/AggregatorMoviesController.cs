using ChithraChalanam.Aggregator.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ChithraChalanam.Aggregator.Api.Controllers;

[ApiController]
[Route("api/aggregator/movies")]
public class AggregatorMoviesController : ControllerBase
{
    private readonly IHttpClientFactory httpClientFactory;

    public AggregatorMoviesController(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMovieWithCast(int id)
    {
        HttpClient movieClient = httpClientFactory.CreateClient("MovieService");
        HttpClient castClient = httpClientFactory.CreateClient("CastService");

        string? authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            movieClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authorizationHeader);
        }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        HttpResponseMessage movieResponse = await movieClient.GetAsync($"/api/movies/{id}");
        if (!movieResponse.IsSuccessStatusCode)
        {
            string movieError = await movieResponse.Content.ReadAsStringAsync();
            return StatusCode((int)movieResponse.StatusCode, movieError);
        }

        string movieJson = await movieResponse.Content.ReadAsStringAsync();
        ChithraChalanam.Aggregator.Api.Dtos.MovieDetailsResponse? movie = JsonSerializer.Deserialize<MovieDetailsResponse>(movieJson, options);

        HttpResponseMessage castResponse = await castClient.GetAsync($"/api/cast/by-movie/{id}");
        if (!castResponse.IsSuccessStatusCode)
        {
            string castError = await castResponse.Content.ReadAsStringAsync();
            return StatusCode((int)castResponse.StatusCode, castError);
        }

        string castJson = await castResponse.Content.ReadAsStringAsync();
        List<MovieCreditDto>? cast = JsonSerializer.Deserialize<List<MovieCreditDto>>(castJson, options);

        AggregatorMovieResponse response = new AggregatorMovieResponse
        {
            Movie = movie,
            Cast = cast ?? new List<MovieCreditDto>(),
            Links = new AggregatorLinks
            {
                Movie = $"/api/aggregator/movies/{id}",
                Cast = $"/api/aggregator/movies/{id}/cast"
            }
        };

        return Ok(response);
    }

    [HttpGet("{id:int}/cast")]
    public async Task<IActionResult> GetCastOnly(int id)
    {
        HttpClient castClient = httpClientFactory.CreateClient("CastService");

        HttpResponseMessage castResponse = await castClient.GetAsync($"/api/cast/by-movie/{id}");
        if (!castResponse.IsSuccessStatusCode)
        {
            string castError = await castResponse.Content.ReadAsStringAsync();
            return StatusCode((int)castResponse.StatusCode, castError);
        }

        string castJson = await castResponse.Content.ReadAsStringAsync();

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        List<MovieCreditDto>? cast = JsonSerializer.Deserialize<List<MovieCreditDto>>(castJson, options);

        return Ok(cast ?? new List<MovieCreditDto>());
    }
}
