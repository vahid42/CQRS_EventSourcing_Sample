using AccountFlow.Api.V2.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Api.V2.Data
{
    public class AccountDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<AccountSnapshot> AccountSnapshots {  get; set; }

        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        {
            //"C:\\Users\\Laptop-V\\AppData\\Local\\Order.db"
            this.DbPath = Init();

            //Add-Migration InitialCreate
            //Update-Database
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
  
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events");
                entity.HasKey(e => e.Id);

            });
            modelBuilder.Entity<AccountSnapshot>(entity =>
            {
                entity.ToTable("AccountSnapshots");
                entity.HasKey(e => e.Id);

            });

        }

        private string DbPath { get; }
        private static string Init()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            return System.IO.Path.Join(path, "AccountFlowV2.db");
        }
    }
}
