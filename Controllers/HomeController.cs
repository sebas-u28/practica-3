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

public async Task<IActionResult> PostDetail(int id)
{
    var posts = await _postIntegration.GetAllPostsAsync();
    var post = posts.FirstOrDefault(p => p.Id == id);

    if (post == null)
        return NotFound();

    var user = await _postIntegration.GetUserByIdAsync(post.UserId);
    var comments = await _postIntegration.GetCommentsByPostIdAsync(id);

    var viewModel = new PostDetailViewModel
    {
        Post = post,
        User = user,
        Comments = comments
    };

    return View(viewModel);
}



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
