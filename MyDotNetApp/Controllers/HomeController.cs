using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyDotNetApp.Models;

namespace MyDotNetApp.Controllers;

public class HomeController : Controller
{

    private readonly AppDbContext context;

    public HomeController(AppDbContext context)
    {
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

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Services()
    {
        var products = context.Products.ToList();

        return View(products);
    }

    public IActionResult Blog()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
