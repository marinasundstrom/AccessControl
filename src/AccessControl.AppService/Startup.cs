using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccessControl.AppService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.NotificationHubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AccessControl.AppService.Persistence;
using AccessControl.AppService.Application.Login;
using AccessControl.AppService.Application.Services;
using FluentValidation;
using AccessControl.AppService.Application.Hubs;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace AccessControl.AppService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddDbContext<AccessControlContext>
                (options => {
                    options.UseSqlite(Configuration["ConnectionStrings:DefaultConnection"],
                    options2 => options2.MigrationsAssembly(typeof(AccessControlContext).AssemblyQualifiedName));
                    options.EnableSensitiveDataLogging();
                })
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AccessControlContext>();

            services.AddCors(options => 
            {
                options.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());

                // options.AddPolicy("AllowSpecificOrigin",
                //   builder => builder.WithOrigins("https://localhost:44387"));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                });

                options.AddPolicy("AdministratorsOnly", policy => policy.RequireClaim("Administrator"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            Microsoft.Extensions.Primitives.StringValues accessToken = context.Request.Query["access_token"];

                            //if (!string.IsNullOrEmpty(accessToken) &&
                            //    (context.HttpContext.WebSockets.IsWebSocketRequest || context.Request.Headers["Accept"] == "text/event-stream"))
                            //{
                            //    context.Token = context.Request.Query["access_token"];
                            //}
                            // If the request is for our hub...

                            PathString path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken))
                            {

                                //(path.StartsWithSegments("/alarms-notifications-hub"))
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddOpenApiDocument();

            services.AddSingleton(sp => NotificationHubClient.CreateClientFromConnectionString(Configuration["Notifications:ConnectionString"], Configuration["Notifications:Path"]));
            services.AddSingleton(sp => ServiceClient.CreateFromConnectionString(Configuration["Hub:ConnectionString"]));
            services.AddSingleton(sp => EventHubClient.CreateFromConnectionString(Configuration["Events:ConnectionString"]));

            services.AddSingleton<DeviceController>();
            services.AddHostedService<AlarmService>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();

            services.AddSingleton<IAccessLogNotifier, AccessLogNotifier>();
            services.AddSingleton<IAccessLogger, AccessLogger>();

            services.AddSignalR();

            services.AddMediatR(typeof(AuthCommand));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

            //services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // TODO: Re-enable !
            //app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            app.UseRouting();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
                builder.MapHub<AlarmNotificationsHub>("/alarms-notifications-hub");
                builder.MapHub<AccessLogHub>("/accesslog");
            });
        }
    }
}
