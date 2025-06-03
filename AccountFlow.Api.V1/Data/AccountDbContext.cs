using AccountFlow.Api.V1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AccountFlow.Api.V1.Data
{
    public class AccountDbContext : DbContext
    {
        public DbSet<Entities.Account> Accounts { get; set; }


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
            modelBuilder.Entity<Entities.Account>(entity =>
            {
                entity.ToTable("Accounts");
                entity.HasKey(e => e.Id);
                entity.Property(p => p.RowVersion)
                    .IsRowVersion()
                    .HasDefaultValue(new byte[8])
                    .ValueGeneratedOnAddOrUpdate();


            });

        }

        private string DbPath { get; }
        private static string Init()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            return System.IO.Path.Join(path, "AccountFlowV1.db");
        }
    }
}
