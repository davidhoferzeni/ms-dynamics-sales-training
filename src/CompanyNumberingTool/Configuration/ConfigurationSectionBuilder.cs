using Microsoft.Extensions.Configuration;

public static class ConfigurationSectionBuilder<T> where T : class, IConfigurationSection, new()
{
    public static T? GetConfigurationSection(IConfiguration configuration, string sectionName) {
        if(string.IsNullOrEmpty(sectionName)) {
            throw new Exception("No section name avialable!");
        }
        var configurationSection = configuration?.GetSection(sectionName)?.Get<T>() ?? new T();
        ConfigurationPromptManager.FillConfigurationSection(configurationSection);
        return configurationSection;
    }
}

