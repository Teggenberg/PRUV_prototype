using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PRUV_WebApp.Models;

namespace PRUV_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PRUV_WebApp.Models.Request>? Request { get; set; }
        public DbSet<PRUV_WebApp.Models.Brand>? Brand { get; set; }

        //public DbSet<PRUV_WebApp.Models.RequestView>? RequestViews { get; set; }

        public DbSet<PRUV_WebApp.Models.UserRequest>? UserRequest { get; set; }

        //public DbSet<PRUV_WebApp.Models.RequestView>? RequestViews { get; set; }

        public DbSet<PRUV_WebApp.Models.StoreUser>? StoreUser { get; set; }

        //public DbSet<PRUV_WebApp.Models.RequestView>? RequestViews { get; set; }

        public DbSet<PRUV_WebApp.Models.CaseOption>? CaseOption { get; set; }
    }
}