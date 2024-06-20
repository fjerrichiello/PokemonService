using Nvisia.PokemonReview.Api.Options;

namespace Nvisia.PokemonReview.Api.Utils;

public static class SqlUtils
{
    public static string GetSqlConnectionString(IConfiguration configuration)
    {
        var postgresOptions = OptionsUtils.GetValidatedOptions<PostgresOptions>(configuration);

        var serverName = postgresOptions.ServerName;
        var port = postgresOptions.Port;
        var username = postgresOptions.UserName;
        var password = postgresOptions.Password;
        var databaseName = postgresOptions.DatabaseName;

        return $"Server={serverName};Port={port};User ID={username};Password={password};Database={databaseName};";
    }
}