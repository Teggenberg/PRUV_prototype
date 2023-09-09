using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Web;
using System.Drawing;
using System.Linq;

namespace PRUV_WebApp.Models
{
    public class RequestView:DbContext
    {
        public DbSet<UserRequest>? Request { get; set; }

        public DbSet<Brand>? Brand { get; set; }

        //public RequestView() { }

    }
}
