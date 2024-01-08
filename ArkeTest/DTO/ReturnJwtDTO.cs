using System.Net;

namespace ArkeTest.DTO
{
    public class ReturnJwtDTO
    {
        public string Message { get; set; } = null!;
        public HttpStatusCode StatusCode { get; set; }
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
