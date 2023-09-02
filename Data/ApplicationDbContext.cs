using Microsoft.EntityFrameworkCore;
using Open.ManifestToolkit.API.Entities;

namespace Open.ManifestToolkit.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Manifest> Manifests { get; set; }
        public DbSet<Entities.Environment> Environments { get; set; }
        public DbSet<SceInstance> SceInstances { get; set; }
    }
}
