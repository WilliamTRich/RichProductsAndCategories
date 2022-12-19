using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RichProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace RichProductsAndCategories.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    //VIEWS
    [HttpGet("")]
    public IActionResult Index()
    {
        ViewBag.AllProducts = _context.Products.ToList();
        return View();
    }
    [HttpGet("categories")]
    public IActionResult Categories()
    {
        ViewBag.AllCategories = _context.Categories.ToList();
        return View();
    }
    [HttpGet("products/{id}")]
    public IActionResult SingleProduct(int id)
    {
        Product? SP = _context.Products.Include(c => c.Categories).ThenInclude(c => c.Category).FirstOrDefault(p => p.ProductId == id);
        MyViewModel viewModel = new MyViewModel
        {
            Product = SP,
            Associations = SP.Categories,
            Categories = _context.Categories.ToList()
        };
        ViewBag.categoriesNotInSP = _context.Categories.Where(c => !c.Products.Any(p => p.Product == SP)).ToList();
        return View("SingleProduct", viewModel);
    }
    [HttpGet("categories/{id}")]
    public IActionResult SingleCategory(int id)
    {
        Category? SC = _context.Categories.Include(p => p.Products).ThenInclude(p => p.Product).FirstOrDefault(p => p.CategoryId == id);
        MyViewModel viewModel = new MyViewModel
        {
            Category = SC,
            Associations = SC.Products,
            Products = _context.Products.ToList()
        };
        ViewBag.productsNotInSP = _context.Products.Where(c => !c.Categories.Any(p => p.Category == SC)).ToList();
        return View("SingleCategory", viewModel);
    }

    //CREATE
    [HttpPost("product/create")]
    public IActionResult CreateProduct(Product newProduct)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            return View("Index");
        }
    }
    [HttpPost("category/create")]
    public IActionResult CreateCategory(Category newCategory)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            return View("Index");
        }
    }
    [HttpPost("product/category/add")]
    public IActionResult AddCategory(MyViewModel newAssociation)
    {
        _context.Add(newAssociation.Association);
        _context.SaveChanges();
        return Redirect($"/products/{newAssociation.Association.ProductId}");
    }
    [HttpPost("category/product/add")]
    public IActionResult AddProduct(MyViewModel newAssociation)
    {
        _context.Add(newAssociation.Association);
        _context.SaveChanges();
        return Redirect($"/categories/{newAssociation.Association.CategoryId}");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
