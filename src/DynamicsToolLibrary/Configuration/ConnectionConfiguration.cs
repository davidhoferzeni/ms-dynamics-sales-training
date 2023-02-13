public sealed class ConnectionConfiguration : IConfigurationSection
{
    public bool IsPropertyRequired(string propertyName) {
        switch (AuthType)
        {
            case AuthType.OAuth:
                var optionaOauthList = new string[] { nameof(ClientSecret)};
                return !optionaOauthList.Contains(propertyName);
            case AuthType.ClientSecret:
                var optionalClientSecretList = new string[] { nameof(UserName), nameof(Password), nameof(RedirectUri), nameof(LoginPrompt)};
                return !optionalClientSecretList.Contains(propertyName);
            default:
                return true;
        }
    }

    public AuthType AuthType { get; set; }  = AuthType.OAuth;
    public string? Url  { get; set; }
    public string? UserName  { get; set; }
    public string? Password  { get; set; }
    public string AppId { get; set; } = "51f81489-12ee-4a9e-aaae-a2591f45987d";
    public string? ClientSecret { get; set; }
    public string RedirectUri { get; set; } = "app://58145B91-0C36-4500-8554-080854F2AC97";
    //public LoginPromptMode LoginPrompt = LoginPromptMode.Auto;
    public LoginPromptMode LoginPrompt { get; set; } = LoginPromptMode.Auto;
    public bool RequireNewInstance { get; set; } = true;
    // This service connection string uses the info provided above.
    // The AppId and RedirectUri are provided for sample code testing.
    public string ConnectionString { get {
        return $@"
            AuthType = {AuthType};
            Url = {Url};
            UserName = {UserName};
            Password = {Password};
            RedirectUri = {RedirectUri};
            LoginPrompt={LoginPrompt};
            AppId = {AppId};
            ClientSecret = {ClientSecret};
            RequireNewInstance = {RequireNewInstance}";
        }
    }
}