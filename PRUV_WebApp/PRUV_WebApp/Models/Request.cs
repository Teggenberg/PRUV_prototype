using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PRUV_WebApp.Models
{
    public class Request
    {
        public int RequestID { get; set; }
        public int RequestYear { get; set; }
        public int BrandId { get; set; }
        public string RequestBrand { get; set; }
        public string RequestModel { get; set; }

        public string NewBrand { get; set; }

        public Request()
        {
                
        }
    }
}
