
using Microsoft.EntityFrameworkCore;
using TailBuddys.Hubs;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<TailBuddysContext>(options => options.UseSqlServer("Server=MOSHIKO\\SQLEXPRESS;Database=TailBuddys;Trusted_Connection=True;TrustServerCertificate=True;"));

            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          .SetIsOriginAllowed(origin => true); // Allow any origin
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/NotificationHub");
            });

            app.Run();
        }
    }
}
