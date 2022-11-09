namespace StuffyHelper.EmailService.Core.Configs
{
    public class EmailServiceConfiguration
    {
        public const string DefaultSectionName = "EmailService";
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
