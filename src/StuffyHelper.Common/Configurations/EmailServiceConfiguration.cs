namespace StuffyHelper.Common.Configurations
{
    /// <summary>
    /// Email service configuration
    /// </summary>
    public record EmailServiceConfiguration
    {
        /// <summary>
        /// Default section name
        /// </summary>
        public const string DefaultSectionName = "EmailService";
        
        /// <summary>
        /// Email server
        /// </summary>
        public string Server { get; init; } = string.Empty;
        
        /// <summary>
        /// Server port
        /// </summary>
        public int Port { get; init; }
        
        /// <summary>
        /// Name who send the email
        /// </summary>
        public string SenderName { get; init; } = string.Empty;
        
        /// <summary>
        /// Email who send the email
        /// </summary>
        public string SenderEmail { get; init; } = string.Empty;
        
        /// <summary>
        /// Account name
        /// </summary>
        public string Account { get; init; } = string.Empty;
        
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; init; } = string.Empty;
    }
}
