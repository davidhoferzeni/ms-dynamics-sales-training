public sealed class StartupConfiguration : IConfigurationSection
{
    public uint? InitialCompanyIndex { get; set; }
    public bool? NonInteractive { get; set; } = true;
}