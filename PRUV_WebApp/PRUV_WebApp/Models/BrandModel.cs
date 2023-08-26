using Microsoft.AspNetCore.Mvc.Rendering;

namespace PRUV_WebApp.Models
{
    public class BrandModel
    {
        public List<SelectListItem> Brands { get; set; }
        public int? BrandId { get; set; }
        public int? Quantity { get; set; }
    }
}
