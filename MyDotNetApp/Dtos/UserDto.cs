using System.ComponentModel.DataAnnotations;

public class UserDto{

    [Required,MaxLength(20)]
    public string Name { get; set; }
    [Required,EmailAddress]
    public String Email { get; set; }
    [Required]
    public string Password { get; set; }

    public string? Role { get; set; } 
}