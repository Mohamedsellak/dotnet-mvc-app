using System.ComponentModel.DataAnnotations;

public class ProductDto 
{
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    public IFormFile? Image { get; set; }
}