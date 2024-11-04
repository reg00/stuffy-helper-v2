namespace StuffyHelper.Common.Configurations
{
    public record EmailServiceConfiguration
    {
        public const string DefaultSectionName = "EmailService";
        public string Server { get; init; } = string.Empty;
        public int Port { get; init; }
        public string SenderName { get; init; } = string.Empty;
        public string SenderEmail { get; init; } = string.Empty;
        public string Account { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
