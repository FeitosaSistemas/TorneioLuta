using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text.Json;
using TorneioLuta.IoC;
using TorneioLuta.Services.MappingProfiles;

namespace TorneioLuta
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();

            if (string.IsNullOrEmpty(env))
                env = "";

            var baseDirectory = Directory.GetCurrentDirectory();

            Configuration = new ConfigurationBuilder()
                .SetBasePath(baseDirectory)
                .AddJsonFile(string.Format("appsettings.{0}.json", env), optional: true, reloadOnChange: true)
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddAutoMapper(x => x.AddProfile(new MappingProfiles()));

            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    });

            services.AddSingleton<IConfiguration>(Configuration);
            services.RegisterNativeInjector();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
