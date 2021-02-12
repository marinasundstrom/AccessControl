using System;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Device.Pwm.Drivers;
using System.Device.Spi;
using System.Device.Spi.Drivers;
using System.Net.Http;
using System.Threading;
using AccessControl.AppService;
using AccessPoint.Components;
using AccessPoint.HostedServices;
using AccessPoint.Models;
using AccessPoint.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AccessPoint
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<AccessPointContext>
                (options => {
                    options.UseSqlite(Configuration["ConnectionStrings:DefaultConnection"],
                    options2 => options2.MigrationsAssembly(typeof(AccessPointContext).AssemblyQualifiedName));
                    options.EnableSensitiveDataLogging();
                })
                .AddDefaultIdentity<User>()
                .AddEntityFrameworkStores<AccessPointContext>();

            services.AddSingleton(sp => DeviceClient.CreateFromConnectionString(Configuration["Hub:ConnectionString"]));

            services.AddSingleton(sp => new GpioController());
            services.AddSingleton(sp => new PwmController(new SoftPwm()));

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;

            services.AddSingleton(sp => ClientFactory.CreateAuthorizationClient("http://192.168.1.139:5000", new HttpClient(new HttpClientHandler()) { Timeout = TimeSpan.FromSeconds(5) }, async () => string.Empty));

            services.AddSingleton<IBuzzerService, BuzzerService>();
            services.AddSingleton<ILEDService, LEDService>();
            services.AddSingleton<IRelayControlService, RelayControlService>();
            services.AddSingleton<ISwitchService, SwitchService>();
            services.AddSingleton<ICommandReceiver, CommandReceiver>();
            services.AddSingleton<IServiceEventClient, ServiceEventClient>();

            services.AddSingleton<IRfidReader>(sp =>
            {
                var connection = new SpiConnectionSettings(0, 0);
                connection.ClockFrequency = 500000;
                var spi = new UnixSpiDevice(connection);
                var logger = sp.GetService<ILogger<RfidReader>>();
                return new RfidReader(logger, spi);
            });

            services.AddSingleton<IAccessPointService, AccessPointService>();
            services.AddHostedService<AccessPointHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
