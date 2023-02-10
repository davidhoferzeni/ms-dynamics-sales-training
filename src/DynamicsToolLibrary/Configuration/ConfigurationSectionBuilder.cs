using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public static class ConfigurationSectionBuilder<T> where T : class, IConfigurationSection, new()
{
    public static T GetConfigurationSection(IConfiguration configuration, IDynamicsToolLogger logger, IDynamicsToolInput inputManager, string sectionName) {
        if(string.IsNullOrEmpty(sectionName)) {
            var errorMessage = "No section name avialable!";
            logger.LogError(errorMessage);
            throw new Exception(errorMessage);
        }
        var configurationSection = configuration?.GetSection(sectionName)?.Get<T>() ?? new T();
        ConfigurationPromptManager.FillConfigurationSection(configurationSection, logger, inputManager);
        return configurationSection;
    }
}

