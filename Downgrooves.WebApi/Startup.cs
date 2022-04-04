using System;
using System.Text;
using Downgrooves.Persistence;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service;
using Downgrooves.Service.Interfaces;
using Downgrooves.WebApi.Config;
using Downgrooves.WebApi.Policies;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace Downgrooves.WebApi
{
    public class Startup
    {
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        public Startup(IWebHostEnvironment env)
        {
            // Config the app to read values from appsettings base on current environment value.
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddUserSecrets("88b85228-82be-4b94-97c7-b18068f8e5fc")
                .AddEnvironmentVariables().Build();
            WebHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                // Disable authentication and authorization in Development.
                services.TryAddSingleton<IPolicyEvaluator, DisableAuthenticationPolicyEvaluator>();
            }

            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "CORS_POLICY",
                    builder =>
                    {
                        builder.WithOrigins(Configuration["AppConfig:WebAppUrl"].Trim('/', '\\'))
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services.AddProblemDetails(setup =>
            {
                setup.IncludeExceptionDetails = (ctx, env) => WebHostEnvironment.IsDevelopment();
            });

            services.AddControllers();
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Downgrooves.WebApi", Version = "v1" });
            });

            services.AddDbContext<DowngroovesDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DatabaseConnection"), sqlOptions => sqlOptions.CommandTimeout(120));
            }
            );

            services.AddScoped<Func<DowngroovesDbContext>>((provider) => () => provider.GetService<DowngroovesDbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMixService, MixService>();

            services.AddScoped<IITunesService, ITunesService>();
            services.AddScoped<IReleaseService, ReleaseService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddTransient<ITokenService, TokenService>();

            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
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

            app.UseProblemDetails();

            app.UseRouting();
            app.UseCors("CORS_POLICY");

            app.Use(async (httpContext, next) =>
            {
                httpContext.Request.Headers[HeaderNames.XRequestedWith] = "XMLHttpRequest";
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("CORS_POLICY");
            });
        }
    }
}