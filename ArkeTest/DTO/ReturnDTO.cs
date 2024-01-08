using System.Net;

namespace ArkeTest.DTO
{
    public class ReturnDTO
    {
        public string Message { get; set; } = null!;
        public HttpStatusCode StatusCode { get; set; }
    }
}
