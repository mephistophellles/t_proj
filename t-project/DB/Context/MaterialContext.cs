using Microsoft.EntityFrameworkCore;
using t_project.Database;
using t_project.Models;

namespace t_project.DB.Context
{
    public class MaterialContext : DbContext
    {
        public DbSet<Material> Materials { get; set; }

        public MaterialContext()
        {
            Database.EnsureCreated();
            Materials.Load();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Config.connection, Config.version);
        }
    }
}