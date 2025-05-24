using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica.Data;
using practica.Models;

namespace practica.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/feedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAll()
        {
            return await _context.DbSetFeedback.ToListAsync();
        }

        // GET: api/feedback/exist/5
        [HttpGet("exist/{postId}")]
        public async Task<ActionResult<bool>> FeedbackExists(int postId)
        {
            var exists = await _context.DbSetFeedback.AnyAsync(f => f.PostId == postId);
            return Ok(exists);
        }

        // POST: api/feedback
        [HttpPost]
        public async Task<IActionResult> PostFeedback([FromBody] Feedback feedback)
        {
            if (await _context.DbSetFeedback.AnyAsync(f => f.PostId == feedback.PostId))
            {
                return BadRequest("Ya existe feedback para este post.");
            }

            _context.DbSetFeedback.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
