namespace StuffyHelper.EmailService.Core.Configs
{
    public class EmailServiceConfiguration
    {
        public const string DefaultSectionName = "EmailService";
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
