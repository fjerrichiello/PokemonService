using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nvisia.PokemonReview.Api.Clients;
using Nvisia.PokemonReview.Api.Domain.Services;
using Nvisia.PokemonReview.Api.Endpoints;
using Nvisia.PokemonReview.Api.Endpoints.Models;
using Nvisia.PokemonReview.Api.Endpoints.Validators;
using Nvisia.PokemonReview.Api.Mappers;
using Nvisia.PokemonReview.Api.Options;
using Nvisia.PokemonReview.Api.Persistence;
using Nvisia.PokemonReview.Api.Persistence.Models;
using Nvisia.PokemonReview.Api.Persistence.Repositories;
using Nvisia.PokemonReview.Api.Utils;

namespace Nvisia.PokemonReview.Api;

public static class StartupExtensions
{
    public static WebApplication ConfigureApi(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            //.WithOrigins("https://localhost:44351))
            .SetIsOriginAllowed(_ => true));

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapApiEndpoints();
        return app;
    }

    public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/user")
            .MapUserApi();

        app.MapGroup("/api/pokemon")
            .MapPokemonApi()
            .RequireAuthorization();

        return app;
    }

    public static IServiceCollection ConfigureApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddCors();

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        var jwtOptions = OptionsUtils.GetValidatedOptions<JwtOptions>(configuration);
        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(jwtOptions.SigningKey)
                    )
                };
            });
        return services;
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<PokemonStoreContext>(options =>
            options.UseNpgsql(SqlUtils.GetSqlConnectionString(configuration)).UseLowerCaseNamingConvention());

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<UserEntity>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<PokemonStoreContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ValidateAndBindOptions<JwtOptions>(configuration);
        return services;
    }

    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddSingleton<IPokemonMapper, PokemonMapper>();
        services.AddSingleton<IUserMapper, UserMapper>();
        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IPokemonService, PokemonService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IPokemonRepository, PokemonRepository>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<CreatePokemonRequest>, CreatePokemonRequestValidator>();
        services.AddSingleton<IValidator<GetAllPokemonRequest>, GetAllPokemonRequestValidator>();
        services.AddSingleton<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddSingleton<IValidator<RegisterRequest>, RegisterRequestValidator>();
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiClient<IPokeApiClient, PokeApiClient, PokeApiOptions>(configuration);
        return services;
    }

    public static IServiceCollection AddApiClient<TInterface, TImplementation, TOptions>(
        this IServiceCollection services, IConfiguration configuration)
        where TInterface : class
        where TImplementation : class, TInterface
        where TOptions : OptionsBase, new()
    {
        var (options, section) = OptionsUtils.GetValidatedOptionsWithSection<TOptions>(configuration);
        var baseUrl = section.GetValue<string>("BaseUrl")!;
        services.ValidateAndBindOptions<TOptions>(configuration);

        services.AddHttpClient(options.Key)
            .ConfigureHttpClient(cfg => { cfg.BaseAddress = new Uri(baseUrl); });

        services.AddSingleton<TInterface, TImplementation>();
        return services;
    }


    public static IServiceCollection ValidateAndBindOptions<TOptions>(this IServiceCollection services,
        IConfiguration configuration)
        where TOptions : OptionsBase, new()
    {
        var section = OptionsUtils.GetValidatedOptionsSection<TOptions>(configuration);
        services.AddOptions<TOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services;
    }
}