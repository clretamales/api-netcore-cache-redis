using System.Text;
using Arquetipo.Api.Configuration;
using Arquetipo.Api.Controllers;
using Arquetipo.Api.Handlers;
using Arquetipo.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;

namespace Arquetipo.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddCustomMvc(Configuration)
                .AddHttpServices();
        services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.WriteIndented = true;
                });
        services.AddOptions<ConnectionStrings>()
            .Bind(Configuration.GetSection(ConnectionStrings.Seccion))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {

        // Configure the HTTP request pipeline.
        // if (env.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        //app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                string swaggerBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"{swaggerBasePath}/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
    {
        //services.
        services.AddOptions();
        services.AddScoped<IRandomHandler, RandomHandler>();
        services.AddScoped<IUsuarioHandler, UsuarioHandler>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        // services.AddScoped<IHandlerRandom4, HandlerRandom4>();

        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("PermitirApiRequest",
            builder => builder.WithOrigins("*").WithMethods("GET","POST","PUT").AllowAnyHeader());
        });

        services.AddApiVersioning(setup =>
        {
            setup.DefaultApiVersion = new ApiVersion(1, 0);
            setup.AssumeDefaultVersionWhenUnspecified = true;
            setup.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'V";
            setup.SubstituteApiVersionInUrl = true;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // options.Authority = "http://192.168.1.102"; // Cambia esto por la dirección del emisor de tus tokens (por ejemplo, tu servidor de identidad o SSO).
            options.Audience = configuration["Jwt:Issuer"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])), // valida key de token local
                ValidateIssuer = true, // por defecto siempre es true
                ValidAudience = configuration["Jwt:Issuer"],
                ValidateAudience = true, // por defecto siempre es true
                ValidateLifetime = true
            };
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.AddCors(options =>
        {
            options.AddPolicy("PermitirApiRequest",
            builder => builder.WithOrigins("*").WithMethods("GET","POST","PUT").AllowAnyHeader());
        });

        return services;
    }

    public static IServiceCollection AddHttpServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        // services.AddHttpClient<IComisionApiClient, ComisionApiClient>();
        // services.AddHttpClient<IFSatApiClient, FSatApiClient>();
        // services.AddHttpClient<IPagoComisionApiClient, PagoComisionApiClient>();
        return services;
    }
    /* private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)));
    } */
}