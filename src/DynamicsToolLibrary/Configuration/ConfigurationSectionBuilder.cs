using Microsoft.Extensions.Configuration;

public static class ConfigurationSectionBuilder<T> where T : class, IConfigurationSection, new()
{
    public static T GetConfigurationSection(IConfiguration configuration, string sectionName) {
        if(string.IsNullOrEmpty(sectionName)) {
            var errorMessage = "No section name avialable!";
            ConsoleWriter.WriteError(errorMessage);
            throw new Exception(errorMessage);
        }
        var configurationSection = configuration?.GetSection(sectionName)?.Get<T>() ?? new T();
        ConfigurationPromptManager.FillConfigurationSection(configurationSection);
        return configurationSection;
    }
}

