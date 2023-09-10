using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace PRUV_WebApp.Models
{
    
    public class StoreUser
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int Store { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
