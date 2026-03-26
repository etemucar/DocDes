namespace DocDes.Service.Dtos.Responses;

public class AuthResult
{
    public string AccessToken { get; set; } = null!;
    public DateTime AccessTokenExpiration { get; set; }
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpiration { get; set; }
}