
using AccountFlow.Api.V2.CQRS;
using AccountFlow.Api.V2.Data;
using AccountFlow.Api.V2.Entities;
using AccountFlow.Api.V2.Repository;

namespace AccountFlow.Api.V2
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
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<ISnapshotRepository, SnapshotRepository>();


            builder.Services.AddCommandHandlers(typeof(Program));
            builder.Services.AddQueryHandlers(typeof(Program));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
