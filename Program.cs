
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Runtime.InteropServices;
using System.Text;
using TailBuddys.Application.Interfaces;
using TailBuddys.Application.Services;
using TailBuddys.Core.Interfaces;
using TailBuddys.Hubs;
using TailBuddys.Hubs.HubInterfaces;
using TailBuddys.Infrastructure.Data;
using TailBuddys.Infrastructure.Services;

namespace TailBuddys
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Limit ASP.NET logs
                .MinimumLevel.Override("System", LogEventLevel.Warning)    // Limit System logs
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error) // Optional: EF Core logs only on error
                .MinimumLevel.Information() // This is the base level for your app
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/backend-log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting up the TailBuddys backend...");

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

                // Add services to the container

                string? ConnectionString = Environment.GetEnvironmentVariable("TailBuddysDBString", EnvironmentVariableTarget.User);

                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new Exception("connection string is required");
                }
                builder.Services.AddControllers();
                builder.Services.AddDbContext<TailBuddysContext>(options => options.UseSqlServer(ConnectionString));

                // Register ActiveDogs as a singleton
                builder.Services.AddSingleton<IDogConnectionTracker, DogConnectionTracker>();
                //builder.Services.AddSingleton<HashSet<int>>();

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
                builder.Services.AddScoped<IImageRepository, ImageRepository>();
                builder.Services.AddScoped<IImageService, ImageService>();
                builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
                builder.Services.AddScoped<INotificationService, NotificationService>();
                builder.Services.AddScoped<IOpenAiService, OpenAiService>();


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
                                "31cb3b1a-f4f3-466e-9099-d4f49a0dd4b8"))// Consider securing this via secret storage
                        };

                        // Allow SignalR access token from query string
                        options.Events = new JwtBearerEvents
                        {
                            // GPT review 
                            OnMessageReceived = context =>
                            {
                                var accessToken = context.Request.Query["access_token"];
                                var path = context.HttpContext.Request.Path;

                                if (!string.IsNullOrEmpty(accessToken) &&
                                    (path.StartsWithSegments("/NotificationHub") || path.StartsWithSegments("/ChatHub")))
                                {
                                    context.Token = accessToken.ToString().Replace("Bearer ", "");
                                }

                                return Task.CompletedTask;
                            },
                            //
                            OnAuthenticationFailed = context =>
                            {
                                Log.Warning("Authentication failed: {Message}", context.Exception.Message);
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {
                                Log.Information("Token validated successfully.");
                                return Task.CompletedTask;
                            }
                        };
                    });

                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("MustBeAdmin", policy => policy.RequireClaim("IsAdmin", "True"));
                    options.AddPolicy("MustHaveDog", policy
                        => policy.RequireAssertion(context
                        => context.User.Claims.Any(c => c.Type == "DogId")));
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

                app.UseAuthentication();
                app.UseAuthorization();

                //app.MapHub<NotificationHub>("/notificationHub");
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<NotificationHub>("/NotificationHub");
                    endpoints.MapHub<ChatHub>("/ChatHub");
                    endpoints.MapControllers(); // Web API controllers
                });

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}