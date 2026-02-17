using ChithraChalanam.Aggregator.Api.Common;
using ChithraChalanam.Aggregator.Api.Dtos;
using ChithraChalanam.Aggregator.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        ApiResponse<AggregatorMovieResponse> response =
            await aggregatorMovieService.GetMovieWithCastAsync(id);

        return StatusCode(response.StatusCode, response);
    }


    [HttpGet("{id:int}/cast")]
    public async Task<IActionResult> GetCastOnly(int id)
    {
        ApiResponse<List<MovieCreditDto>> response =
            await aggregatorMovieService.GetCastOnlyAsync(id);

        return StatusCode(response.StatusCode, response);
    }

}
