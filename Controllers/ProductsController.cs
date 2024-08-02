using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ProductsController : Controller {

    private readonly AppDbContext context;
    private readonly IWebHostEnvironment environment;

    public ProductsController(AppDbContext context,IWebHostEnvironment environment){
        this.context = context;
        this.environment = environment;
    }

    public IActionResult Index(){

        var products = context.Products.ToList();

        return View(products);
    }

    public IActionResult Create(){
        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductDto productDto){
        if(productDto.Image == null){
            ModelState.AddModelError("Image","Image is required");
        }
        if (!ModelState.IsValid)
        {
            return View(productDto);
        }

        string newfileName = DateTime.Now.ToString("ddMMYYHHmmssfff");
        var extension = Path.GetExtension(productDto.Image.FileName);
        string imageName = newfileName + extension;
        var path = Path.Combine(environment.WebRootPath, "images/products", imageName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            productDto.Image.CopyTo(fileStream);
        }

        Product product = new Product(){
            Name = productDto.Name,
            Price = productDto.Price,
            Image = imageName
        };

        context.Products.Add(product);
        context.SaveChanges();

        
        return RedirectToAction("Index","Products");
    }

    
    public IActionResult Edit(int id){
        Product product = context.Products.Find(id);

        if(product == null){
            return RedirectToAction("Index","Products");
        }

        var productDto = new ProductDto()
        {
            Name = product.Name,
            Price = product.Price,
            // Image = product.Image
        };

        ViewData["ProductId"] = product.Id;
        ViewData["ProductImage"] = product.Image;
        
        return View(productDto);
    }

    [HttpPost]
    public IActionResult Edit(int id, ProductDto productDto)
    {
        // Find the existing product by ID
        Product product = context.Products.Find(id);

        // Check if the product exists
        if (product == null)
        {
            return RedirectToAction("Index", "Products");
        }

        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            ViewData["ProductId"] = product.Id;
            ViewData["ProductImage"] = product.Image;
            return View(productDto);
        }

        // Check if a new image has been uploaded
        if (productDto.Image != null)
        {
            // Generate a new image name with a timestamp to avoid conflicts
            string newImageName = DateTime.Now.ToString("ddMMYYHHmmssfff") + Path.GetExtension(productDto.Image.FileName);

            // Define the path where the new image will be saved
            var newPath = Path.Combine(environment.WebRootPath, "images/products", newImageName);

            // Save the new image
            using (var fileStream = new FileStream(newPath, FileMode.Create))
            {
                productDto.Image.CopyTo(fileStream);
            }

            // Delete the old image if it exists
            if (!string.IsNullOrEmpty(product.Image))
            {
                var oldPath = Path.Combine(environment.WebRootPath, "images/products", product.Image);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            // Update the product's image name
            product.Image = newImageName;
        }

        // Update the product's other properties
        product.Name = productDto.Name;
        product.Price = productDto.Price;

        // Save changes to the database
        context.SaveChanges();

        // Redirect to the index action
        return RedirectToAction("Index", "Products");
    }


    public IActionResult Delete(int id){
        Product product = context.Products.Find(id);
        if(product == null){
            return RedirectToAction("Index","Products");
        }

        string imageFullPath = environment.WebRootPath + "/images/products" + product.Image;
        System.IO.File.Delete(imageFullPath);

        context.Products.Remove(product);
        context.SaveChanges();
        return RedirectToAction("Index","Products");
    }
}