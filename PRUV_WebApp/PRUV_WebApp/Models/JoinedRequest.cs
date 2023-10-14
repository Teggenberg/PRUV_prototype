namespace PRUV_WebApp.Models
{
    public class JoinedRequest
    {
        public int? Id { get; set; }
        public int RequestID { get; set; }
        public int? Store { get; set; }
        public string? Year { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Brand { get; set; }
        public string? Model { get; set; }

        public string? Case { get; set; }
        public string? Created { get; set; }

        public bool? Initiated { get; set; }

        public List<byte[]>? images { get; set; }
    }
}
