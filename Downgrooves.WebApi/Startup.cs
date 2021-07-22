using System;
using Downgrooves.Domain;
using Downgrooves.Persistence;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service;
using Downgrooves.Service.Interfaces;
using Downgrooves.WebApi.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Downgrooves.WebApi
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Configuration = InitConfiguration(env);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Downgrooves.WebApi", Version = "v1" });
            });

            services.AddDbContext<DowngroovesDbContext>(options =>
            {
                options.UseSqlite(AppSettings.ConnectionString, sqlOptions => sqlOptions.CommandTimeout(120));
            }
            );

            services.AddScoped<Func<DowngroovesDbContext>>((provider) => () => provider.GetService<DowngroovesDbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IMixService, MixService>();
            services.AddScoped<IITunesService, ITunesService>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Downgrooves.WebApi v1"));
            }

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IConfiguration InitConfiguration(IWebHostEnvironment env)
        {
            // Config the app to read values from appsettings base on current environment value.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                .AddUserSecrets("88b85228-82be-4b94-97c7-b18068f8e5fc")
                .AddEnvironmentVariables().Build();
            //
            // Map AppSettings section in appsettings.json file value to AppSetting model
            configuration.GetSection("AppSettings").Get<AppSettings>(options => options.BindNonPublicProperties = true);
            return configuration;
        }
    }
}