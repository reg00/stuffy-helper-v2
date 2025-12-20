using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for update user
/// </summary>
public class UpdateModel
{
    /// <summary>
    /// Username
    /// </summary>
    [Required(ErrorMessage = "Необходимо заполнить поле 'UserName'")]
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// User first name
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// User middle name
    /// </summary>
    public string MiddleName { get; init; } = string.Empty;

    /// <summary>
    /// User last name
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// User phone
    /// </summary>
    [Phone(ErrorMessage = "Неверный формат телефона")]
    public string Phone { get; init; } = string.Empty;
}