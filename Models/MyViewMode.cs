#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace RichProductsAndCategories.Models;
public class MyViewModel
{
    public Category Category { get; set; }
    public List<Category> Categories { get; set; } = new List<Category>();

    public Product Product { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();

    public Association Association { get; set; }
    public List<Association> Associations { get; set; } = new List<Association>();
}