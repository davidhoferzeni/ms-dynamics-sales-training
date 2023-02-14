# MS Dynamics 365 Sales Example

This small project collects a few dotnet and projects that were created as part of a small example workflow for Microsoft Dynamics Sales.
Ther are to applications currently available:
- A console application that shows all current accounts and assigns incremental indices to them.
- An Azure Function App project that can be hosted on Azure to provide (limited) CRUD functionality for accounts.

## MS Dynamics Sales Console
Currently, the console application runs exactly one command that assigns an incremental index to all accounts in alphabetical order, starting with a provided start index. Configuration details will be prompted when required, configuration files and environment variables are available for non-interactive scenarios.

## MS Dynamics Sales Azure Function App
The Function App offers basics CRUD functionality through multiple https triggers and a timer based function that assigns an incremental index to all accounts once a day.

## Configuration

Configuring the application can be done in the following ways:
- Configuration File (`appsettings.json`)
- Environment Variables
- Command Line Arguments
- User Input (only if no value was previously provided)

For Azure Functions, only configuration via environment variables (i.e.: Application Settings) is available.

Please note that setting a value through one method above overwrites the previous values (order as stated above).

### Configuration Values

**Mandatory:**

- **ConnectionConfiguration.Url**: Url poiting to your Dynamics Sales Instance
- **ConnectionConfiguration.UserName**: Name of the user to login (only if AuthType is set to `OAuth`)
- **ConnectionConfiguration.Password**: Password of the user required for login (only if AuthType is set to `OAuth`)
- **ConnectionConfiguration.AppId**: Id oa registered service principal (only if AuthType is set to `ClientSecret`)
- **ConnectionConfiguration.ClientSecret**: Secret of the registered service principal (only if AuthType is set to `ClientSecret`)

**Optional:**

- **ConnectionConfiguration.AuthType** Type of authentication used to login in. Available options are: `OAuth` and `ClientSecret`. Defaults to `OAuth`
- **NonInteractive**: If true, will immedtialey throw an error if a mandatory configuration value is not provided. Defaults to `false`.

Please note that *nested* configuration values (separated by .) need to be supplied based on the configuration method:

- **Configuration File:** Nested json properties, e.g.: `{ "StartupConfiguration": { "InitialAccountIndex": 123 } }`
- **Environment Variables:** Requires colon `:` as a separator character, e.g.: `--StartupConfiguration:InitialAccountIndex=123`
- **Command Line Arguments:** Requires double underscores `__` as separator characters, e.g.: `StartupConfiguration__InitialAccountIndex` `123`

### Example applicationsettings.json

```
{
    "ConnectionConfiguration": {
        "Url": "https://orgXXXX.crm4.dynamics.com/",
        "UserName": "username@companyname.onmicrosoft.com",
        "Password": "password123"
    }
}
```

### Example local.host.json

```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "NonInteractive": true,
        "ConnectionConfiguration__Url": "https://org48XXXX.crm4.dynamics.com/",
        "ConnectionConfiguration__AppId": "00000000-0000-0000-0000-000000000000",
        "ConnectionConfiguration__ClientSecret": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        "StartupConfiguration__InitialAccountIndex": 1,
        "ConnectionConfiguration__AuthType": "ClientSecret"
    }
}
```

## Local Development

### Visual Studio Code

The following extensions should be installed in order to successfully run and debug the console application:
- `ms-dotnettools.csharp`

To run either of the applications, navigate to the Debug section of Visual Studio Code and choose the configuration you want to use for debugging, then hit F5.

![image](https://user-images.githubusercontent.com/32648667/218674188-e89a4da2-be04-4be8-adab-da4c75f4e421.png)

### Visual Studio

To get started, clone the repository, open the project file `src/DynamicsToolConsole.csproj` in Visual Studio and press F5 to start the application.
