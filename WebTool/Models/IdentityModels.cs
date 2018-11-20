using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebTool.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public DbSet<Module> Module { get; set; }
        public DbSet<Auth> Auth { get; set; }
        public DbSet<realmNameInfo> realmNameInfo { get; set; }


        public void ExecSql(string sql)
        {
            Database.ExecuteSqlCommand(sql);
        }
    }
}