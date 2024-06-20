using Nvisia.PokemonReview.Api.Options;

namespace Nvisia.PokemonReview.Api.Utils;

public static class OptionsUtils
{
    public static (TOptions options, IConfiguration section) GetValidatedOptionsWithSection<TOptions>(
        IConfiguration configuration)
        where TOptions : OptionsBase, new()
    {
        var (options, section) = GetOptions<TOptions>(configuration);
        options.Validate();
        return (options, section);
    }

    public static TOptions GetValidatedOptions<TOptions>(IConfiguration configuration)
        where TOptions : OptionsBase, new()
    {
        var (options, _) = GetOptions<TOptions>(configuration);
        options.Validate();
        return options;
    }

    public static IConfiguration GetValidatedOptionsSection<TOptions>(IConfiguration configuration)
        where TOptions : OptionsBase, new()
    {
        var (options, section) = GetOptions<TOptions>(configuration);
        options.Validate();
        return section;
    }

    public static (TOptions options, IConfiguration section) GetOptions<TOptions>(IConfiguration configuration)
        where TOptions : OptionsBase, new()
    {
        var section = GetOptionsSection<TOptions>(configuration);
        return (BindOptions<TOptions>(section), section);
    }

    public static IConfiguration GetOptionsSection<TOptions>(IConfiguration configuration)
        where TOptions : OptionsBase, new()
    {
        var options = new TOptions();
        return configuration.GetSection(options.Key);
    }

    public static TOptions BindOptions<TOptions>(IConfiguration configuration)
        where TOptions : OptionsBase, new()
    {
        var config = new TOptions();
        configuration.Bind(config);
        return config;
    }
}