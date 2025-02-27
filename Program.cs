
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

            builder.Services.AddScoped<IAuth, JwtAuthService>();
            builder.Services.AddHttpClient<IGoogleAuthService, GoogleAuthService>();
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

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,

                        ValidIssuer = "TailBuddysServer",
                        ValidAudience = "TailBuddysApp",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            "31cb3b1a-f4f3-466e-9099-d4f49a0dd4b8"))
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                // להוסיף קליימס מתאימים לביצוע בדיקות וולידציה בקונטרולרים
                options.AddPolicy("MustBeAdmin", policy => policy.RequireClaim("isAdmin", "True"));
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

            app.UseCors("myCorsPolicy");

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
