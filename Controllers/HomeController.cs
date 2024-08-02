using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyDotNetApp.Models;

namespace MyDotNetApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext context;

    public HomeController(ILogger<HomeController> logger,AppDbContext context)
    {
        _logger = logger;
        this.context = context;
    }

    public IActionResult Index()
    {
        var products = context.Products.ToList();

        return View(products);
    }

    public IActionResult Shop()
    {
        var products = context.Products.ToList();

        return View(products);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
