using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using practica.Integration.Exchange;
using practica.Models;

namespace practica.Controllers;

public class HomeController : Controller
{
private readonly ILogger<HomeController> _logger;
private readonly PostIntegration _postIntegration;

public HomeController(ILogger<HomeController> logger, PostIntegration postIntegration)
{
    _logger = logger;
    _postIntegration = postIntegration;
}


public async Task<IActionResult> Index()
{
    var posts = await _postIntegration.GetAllPostsAsync();
    return View(posts);
}



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
