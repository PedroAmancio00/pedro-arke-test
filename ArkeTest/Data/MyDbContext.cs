using ArkeTest.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArkeTest.Data
{
    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserInformation> UserInformations { get; set; }
    }
}
