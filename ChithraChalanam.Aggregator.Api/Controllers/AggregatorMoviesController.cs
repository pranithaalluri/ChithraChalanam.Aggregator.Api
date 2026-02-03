using ChithraChalanam.Aggregator.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ChithraChalanam.Aggregator.Api.Dtos;
namespace ChithraChalanam.Aggregator.Api.Controllers;

[ApiController]
[Route("api/aggregator/movies")]
public class AggregatorMoviesController : ControllerBase
{
    private readonly IAggregatorMovieService aggregatorMovieService;

    public AggregatorMoviesController(
        IAggregatorMovieService aggregatorMovieService)
    {
        this.aggregatorMovieService = aggregatorMovieService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMovieWithCast(int id)
    {
        string? accessToken =
            Request.Headers["Authorization"].FirstOrDefault();

        AggregatorMovieResponse? response =
            await aggregatorMovieService
                .GetMovieWithCastAsync(id, accessToken);

        return Ok(response);
    }

    [HttpGet("{id:int}/cast")]
    public async Task<IActionResult> GetCastOnly(int id)
    {
        List<MovieCreditDto>? cast =
            await aggregatorMovieService.GetCastOnlyAsync(id);

        return Ok(cast);
    }
}
