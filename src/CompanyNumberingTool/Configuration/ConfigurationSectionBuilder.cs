using Microsoft.Extensions.Configuration;

public static class ConfigurationSectionBuilder<T> where T : class, IConfigurationSection
{
    public static T? GetConfigurationSection(IConfiguration configuration, string sectionName) {
        if(string.IsNullOrEmpty(sectionName)) {
            throw new Exception("No section name avialable!");
        }
        return configuration?.GetSection(sectionName)?.Get<T>();
    }
}

