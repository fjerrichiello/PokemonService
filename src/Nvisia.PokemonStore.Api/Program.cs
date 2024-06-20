using Nvisia.PokemonReview.Api;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .ConfigureApi(configuration)
    .ConfigureDatabase(configuration)
    .AddOptions(configuration)
    .AddHttpClients(configuration)
    .AddSwagger()
    .AddIdentity()
    .AddValidators()
    .AddMappers()
    .AddDomainServices()
    .AddRepositories();

var app = builder.Build();

app.ConfigureApi()
    .ConfigureSwagger();

app.Run();

public partial class Program;