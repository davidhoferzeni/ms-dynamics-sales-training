public sealed class StartupConfiguration : IConfigurationSection
{
    public uint? InitialAccountIndex { get; set; }
    public bool? NonInteractive { get; set; } = true;
}