namespace PRUV_WebApp.Models
{
    public class JoinedRequest
    {
        public int? Id { get; set; }
        public int RequestID { get; set; }
        public int? Store { get; set; }
        public string? Year { get; set; }

        public string? Brand { get; set; }
        public string? Model { get; set; }

        public string? Case { get; set; }
    }
}
