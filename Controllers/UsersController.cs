using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class UsersController(AppDbContext context) : Controller
{
    private readonly AppDbContext context = context;

    public IActionResult Index(){
        var users = context.Users.ToList();
        return View(users);
    }

    public IActionResult Create(){
        return View();
    }

    [HttpPost]
    public IActionResult Create(UserDto userDto){
    
        if (!ModelState.IsValid)
        {
            return View(userDto);
        }
    
        if(ModelState.IsValid){
    
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                Role = userDto.Role,
            };
    
            context.Users.Add(user);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    
        return View(userDto); // Add this line to return a value in case none of the conditions are met
    }

    public IActionResult Edit(int id){
        var user = context.Users.Find(id);
        if (user == null)
        {
            return RedirectToAction("Index","Users");
        }
        var userDto = new UserDto()
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Role = user.Role,
        };
        return View(userDto);
    }

    [HttpPost]
    public IActionResult Edit(int Id,UserDto userDto){
        var user = context.Users.Find(Id);
        if (user == null)
        {
            return RedirectToAction("Index","Users");
        }
        if (!ModelState.IsValid)
        {
            return View(userDto);
        }

        user.Name = userDto.Name;
        user.Email = userDto.Email;
        user.Password = userDto.Password;
        user.Role = userDto.Role;
        context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int Id){
        var user = context.Users.Find(Id);
        if (user == null)
        {
            return RedirectToAction("Index","Users");
        }
        context.Users.Remove(user);
        context.SaveChanges();
        return RedirectToAction("Index","Users");
    }
}