public sealed class ConnectionConfigurationSection : IConfigurationSection
{
    public required string Url  { get; set; }
    public required string UserName  { get; set; }
    public required string Password  { get; set; }
    public string AppId { get; set; } = "51f81489-12ee-4a9e-aaae-a2591f45987d";
    public string RedirectUri { get; set; } = "app://58145B91-0C36-4500-8554-080854F2AC97";
    //public LoginPromptMode LoginPrompt = LoginPromptMode.Auto;
    public LoginPromptMode LoginPrompt { get; set; } = LoginPromptMode.Auto;
    public bool RequireNewInstance { get; set; } = true;
    // This service connection string uses the info provided above.
    // The AppId and RedirectUri are provided for sample code testing.
    public string ConnectionString { get {
        return $@"
            AuthType = OAuth;
            Url = {Url};
            UserName = {UserName};
            Password = {Password};
            AppId = {AppId};
            RedirectUri = {RedirectUri};
            LoginPrompt={LoginPrompt};
            RequireNewInstance = {RequireNewInstance}";
        }
    }
}