using ChithraChalanam.Aggregator.Api.Dtos;
using ChithraChalanam.Aggregator.Api.Dtos.AuthDto;
using ChithraChalanam.Aggregator.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChithraChalanam.Aggregator.Api.Controllers;

[ApiController]
[Route("api/aggregator/auth")]
public class AggregatorAuthController : ControllerBase
{
    private readonly IAggregatorAuthService aggregatorAuthService;

    public AggregatorAuthController(
        IAggregatorAuthService aggregatorAuthService)
    {
        this.aggregatorAuthService = aggregatorAuthService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        AuthResponseDto response =
            await aggregatorAuthService.LoginAsync(request);

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        AuthResponseDto response =
            await aggregatorAuthService.RegisterAsync(request);

        return Ok(response);
    }
}
