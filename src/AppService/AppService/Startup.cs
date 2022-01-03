using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AppService.Application.AccessLog;
using AppService.Application.AccessLog.Hubs;
using AppService.Application.Alarm;
using AppService.Application.Alarm.Hubs;
using AppService.Application.Devices;
using AppService.Application.Login;
using AppService.Application.Services;
using AppService.Domain.Entities;
using AppService.Infrastructure;
using AppService.Infrastructure.Persistence;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Azure.NotificationHubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace AppService
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

            services.AddInfrastructure();

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddDbContext<AccessControlContext>
                ((sp, options) =>
                {
                    options.UseSqlServer(
                      sp.GetRequiredService<IConfiguration>().GetConnectionString("appservice-db"));
                    options.EnableSensitiveDataLogging();
                })
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AccessControlContext>()
                .AddDefaultTokenProviders();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                          builder => builder
                          .AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
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
                            var accessToken = context.Request.Query["access_token"];

                            if (!string.IsNullOrEmpty(accessToken) &&
                                (context.HttpContext.WebSockets.IsWebSocketRequest || context.Request.Headers["Accept"] == "text/event-stream"))
                            {
                                context.Token = context.Request.Query["access_token"];
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddOpenApiDocument();

            /*
            services.AddSingleton(sp => NotificationHubClient.CreateClientFromConnectionString(Configuration["Notifications:ConnectionString"], Configuration["Notifications:Path"]));
            services.AddSingleton(sp => ServiceClient.CreateFromConnectionString(Configuration["Hub:ConnectionString"]));
            services.AddSingleton(sp => EventHubClient.CreateFromConnectionString(Configuration["Events:ConnectionString"]));
            */

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumers(typeof(AppService.Application.Alarm.AlarmConsumer).Assembly);
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            })
            .AddMassTransitHostedService()
            .AddGenericRequestClient();

            services.AddSingleton<DeviceController>();
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
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("AllowAnyOrigin");

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
