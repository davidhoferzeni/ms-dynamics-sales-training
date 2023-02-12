using System.Configuration;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public static class ConfigurationPromptManager
{
    public static void FillConfigurationSection(IConfigurationSection configurationSection, ILogger logger, IDynamicsToolInput inputManager)
    {
        var configurationProperties = configurationSection.GetType().GetProperties();
        foreach (var configurationProperty in configurationProperties)
        {
            var configurationValue = configurationProperty.GetValue(configurationSection);
            if (configurationValue != null)
            {
                continue;
            }
            var configurationKey = configurationProperty.Name;
            var configurationKeyType = configurationProperty.PropertyType;
            object? newConfigurationValue = null;
            var inputTimeout = 10;
            var inputCounter = 0;
            while (newConfigurationValue == null && inputCounter < inputTimeout)
            {
                logger.LogInformation($"Please enter a value for setting {configurationKey}:");
                string? input = inputManager.GetStringInput();
                Type nonNullableType = Nullable.GetUnderlyingType(configurationKeyType) ?? configurationKeyType;
                try
                {
                    newConfigurationValue = Convert.ChangeType(input, nonNullableType);
                }
                catch (Exception)
                {
                    logger.LogError("Incorrect value, please try again!");
                }
                // only support string and int for now
                if (newConfigurationValue != null)
                {
                    configurationProperty.SetValue(configurationSection, newConfigurationValue);
                }
                inputCounter++;
            }
        }
    }
}

