#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace RichProductsAndCategories.Models;
public class Product
{
    [Key]
    public int ProductId { get; set; }
    [Required(ErrorMessage = "Name of product is required.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Price is required.")]
    public double Price { get; set; }

    public List<Association> Categories { get; set; } = new List<Association>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}