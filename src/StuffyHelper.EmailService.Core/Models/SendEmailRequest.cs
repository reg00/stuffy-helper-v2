namespace StuffyHelper.EmailService.Core.Models;

public record SendEmailRequest
{
    public string Email { get; set; }
    
    public string Subject { get; set; }
    
    public string Message { get; set; }
};