using System;
using System.Linq;
using System.Text;
using Downgrooves.Domain;
using Downgrooves.Service;
using Downgrooves.Service.Interfaces;
using Downgrooves.WebApi.Policies;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Log = Serilog.Log;

namespace Downgrooves.WebApi
{
    public class Startup(IWebHostEnvironment env)
    {
        public IWebHostEnvironment WebHostEnvironment { get; set; } = env;

        public IConfiguration Configuration { get; } = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddUserSecrets("88b85228-82be-4b94-97c7-b18068f8e5fc")
                .AddEnvironmentVariables().Build();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                // Disable authentication and authorization in Development.
                services.TryAddSingleton<IPolicyEvaluator, DisableAuthenticationPolicyEvaluator>();
            }

            services.AddLogging();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "CORS_POLICY",
                    policy =>
                    {
                        var urls = Configuration
                            .GetSection("AppConfig:WebAppUrls")
                            .GetChildren()
                            .Select(x => x.Value.Trim('/', '\\'));
                        policy.WithOrigins([.. urls])
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            services.AddProblemDetails(setup =>
            {
                setup.IncludeExceptionDetails = (ctx, env) => WebHostEnvironment.IsDevelopment();
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSingleton(Configuration);
            services.Configure<AppConfig>(options => Configuration.GetSection(nameof(AppConfig)).Bind(options));

            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IMixService, MixService>();
            services.AddScoped<IReleaseService, ReleaseService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddTransient<ITokenService, TokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console(theme: AnsiConsoleTheme.Literate, applyThemeToRedirectedOutput: true)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .CreateLogger();

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