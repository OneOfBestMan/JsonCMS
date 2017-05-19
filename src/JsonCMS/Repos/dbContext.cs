using Microsoft.EntityFrameworkCore;

namespace JsonCMS
{
    public partial class dbContext : DbContext
    {
        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        /* 200 towns */ 

        public DbSet<JsonCMS.Repos._200Towns.RepoGallery> chosenImagesFromTowns { get; set; }
        public DbSet<JsonCMS.Repos._200Towns.RepoPage> ChosenTowns { get; set; }

        /* Top 100 destinations */

        public DbSet<JsonCMS.Repos._Top100.RepoGallery> chosenImagesFrom100Dest { get; set; }
        public DbSet<JsonCMS.Repos._Top100.RepoPage> Chosen100dest { get; set; }

    }
}
