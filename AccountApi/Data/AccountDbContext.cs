using AccountApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountApi.Data
{
    public class AccountDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Event> Events { get; set; }

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
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Accounts"); // متد ToTable باید در اینجا کار کند  
                entity.HasKey(e => e.Id); // تعریف کلید اصلی  
                // دیگر تنظیمات  
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events"); // همینطور برای EventEntity  
                entity.HasKey(e => e.Id); // تعریف کلید اصلی  
                // دیگر تنظیمات  
            });

        }

        private string DbPath { get; }
        private static string Init()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            return System.IO.Path.Join(path, "Account.db");
        }
    }
}
