using ChithraChalanam.Aggregator.Api.Dtos.AuthDto;
using ChithraChalanam.Aggregator.Api.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace ChithraChalanam.Aggregator.Api.Services;

public class AggregatorAuthService : IAggregatorAuthService
{
    private readonly IHttpClientFactory httpClientFactory;

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AggregatorAuthService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        try
        {
            HttpClient client =
                httpClientFactory.CreateClient("AuthService");

            StringContent content = new(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response =
                await client.PostAsync("/api/auth/login", content);

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<AuthResponseDto>(
                json,
                jsonOptions)!;
        }
        catch (Exception ex)
        {
            throw new Exception("Login failed.", ex);
        }
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        try
        {
            HttpClient client =
                httpClientFactory.CreateClient("AuthService");

            StringContent content = new(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response =
                await client.PostAsync("/api/auth/register", content);

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<AuthResponseDto>(
                json,
                jsonOptions)!;
        }
        catch (Exception ex)
        {
            throw new Exception("Registration failed.", ex);
        }
    }
}
