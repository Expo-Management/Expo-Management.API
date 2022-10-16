namespace Expo_Management.API.Domain.Models.Reponses
{
    public class Response
    {
        public int Status { get; set; }
        public object Message { get; set; } = default!;
        public object Data { get; set; } = default!;
        public string[] Error { get; set; } = Array.Empty<string>();
    }
}
