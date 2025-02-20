
using Microsoft.EntityFrameworkCore;
using TailBuddys.Application.Interfaces;
using TailBuddys.Application.Services;
using TailBuddys.Core.Interfaces;
using TailBuddys.Infrastructure.Data;
using TailBuddys.Infrastructure.Services;

namespace TailBuddys
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container

            string? ConnectionString = Environment.GetEnvironmentVariable("TailBuddysDBString", EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new Exception("connection string is required");
            }
            builder.Services.AddControllers();
            builder.Services.AddDbContext<TailBuddysContext>(options => options.UseSqlServer(ConnectionString));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDogRepository, DogRepository>();
            builder.Services.AddScoped<IDogService, DogService>();
            builder.Services.AddScoped<IMatchRepository, MatchRepository>();
            builder.Services.AddScoped<IMatchService, MatchService>();
            builder.Services.AddScoped<IParkRepository, ParkRepository>();
            builder.Services.AddScoped<IParkService, ParkService>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();


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

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //    endpoints.MapHub<NotificationHub>("/NotificationHub");
            //});

            app.MapControllers();


            app.Run();
        }
    }
}
