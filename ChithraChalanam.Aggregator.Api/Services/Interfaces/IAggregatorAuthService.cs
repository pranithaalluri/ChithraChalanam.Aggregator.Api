using ChithraChalanam.Aggregator.Api.Dtos.AuthDto;

namespace ChithraChalanam.Aggregator.Api.Services.Interfaces;

public interface IAggregatorAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<string> RegisterAsync(RegisterRequestDto request);
}
