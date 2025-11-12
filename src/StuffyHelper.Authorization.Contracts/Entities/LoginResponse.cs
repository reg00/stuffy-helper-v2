namespace StuffyHelper.Authorization.Contracts.Entities;

public record LoginResponse(string AccessToken, long ExpiresIn);