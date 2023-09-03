namespace PRUV_WebApp.Models
{
    public class UserRequest
    {
        public int Id { get; set; }

        public int? RequestID { get; set; }
        public string? RequestYear { get; set; }
        public int? BrandId { get; set; }
        
        public string? RequestModel { get; set; }

        public string? Serial { get; set; }

        public int UserID { get; set; }

        public bool Intiated { get; set; }
        public int? InitiatedBy { get; set; }

        public DateTime? InitiatedAt { get; set; }

        public String? Details { get; set; }

        public int? AskingPrice { get; set; }
        public int? Cost { get; set; }
        public float? Retail { get; set; }
        public int Case { get; set; }

        public DateTime Created { get; set; }


        

        public UserRequest()
        {

        }
    }
}
