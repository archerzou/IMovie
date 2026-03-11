namespace Movies.Api;

public class TokenGenerationRequest
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public Dictionary<string, object> CustomClaims { get; init; } = new();
}