
using AccountApi.CQRS;
using AccountApi.CQRS.Comands;
using AccountApi.CQRS.Comands.CommandHandler;
using AccountApi.Data;
using AccountApi.Entities;
using AccountApi.Repository;
using AccountApi.Services;

namespace AccountApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AccountDbContext>();
            builder.Services.AddScoped<IGenericRepository<Account>, GenericRepository<Account>>();
            builder.Services.AddScoped<IGenericRepository<Event>, GenericRepository<Event>>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddCommandHandlers(typeof(Program));
            builder.Services.AddQueryHandlers(typeof(Program));


            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
