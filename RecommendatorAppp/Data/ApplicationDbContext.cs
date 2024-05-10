using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecommendatorAppp.Models;

namespace RecommendatorAppp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Information> Information { get; set; }
        public DbSet<ServiceInformation> ServiceInformation { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
    }
}