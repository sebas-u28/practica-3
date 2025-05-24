using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using practica.Integration.Exchange;
using practica.Integration;
using practica.Models;

namespace practica.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PostIntegration _postIntegration;
    private readonly FeedbackIntegration _feedbackIntegration;

    public HomeController(
        ILogger<HomeController> logger,
        PostIntegration postIntegration,
        FeedbackIntegration feedbackIntegration)
    {
        _logger = logger;
        _postIntegration = postIntegration;
        _feedbackIntegration = feedbackIntegration;
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

    [HttpPost]
    public async Task<IActionResult> SendFeedback(int postId, string sentimiento)
    {
        if (!Enum.TryParse<SentimientoTipo>(sentimiento, true, out var sentimientoEnum))
        {
            return BadRequest("Sentimiento inválido.");
        }

        var feedback = new Feedback
        {
            PostId = postId,
            Sentimiento = sentimientoEnum,
            Fecha = DateTime.UtcNow
        };

        bool result = await _feedbackIntegration.PostFeedbackAsync(feedback);

        if (!result)
        {
            TempData["ErrorMessage"] = "No se pudo enviar el feedback.";
        }

        return RedirectToAction(nameof(PostDetail), new { id = postId });
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
