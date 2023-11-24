using CPULogServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CPULogServer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<CPUDataModel> CPUData { get; set; }
        public DbSet<ClientModel> Clients { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=CPULog.db");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
