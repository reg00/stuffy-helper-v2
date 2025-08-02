namespace StuffyHelper.EmailService.Contracts.Models;

/// <summary>
/// Send email request
/// </summary>
public record SendEmailRequest
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Theme
    /// </summary>
    public string Subject { get; set; }
    
    /// <summary>
    /// Message text
    /// </summary>
    public string Message { get; set; }
};