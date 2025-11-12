namespace StuffyHelper.Authorization.Contracts.Entities;

public class RefreshEntity
{
    public Guid Id { get; init; }
    
    public string UserId { get; init; }
    
    public string Hash { get; init; }
    
    public DateTime ExpiresAt { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public bool Revoked { get; init; }
}