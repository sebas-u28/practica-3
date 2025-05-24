using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using practica.Models;
using practica.Service;

namespace practica.Rest
{
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackApiController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbackApiController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET api/feedback
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Feedback>>> List()
        {
            var feedbacks = await _feedbackService.GetAll();
            if (feedbacks == null || feedbacks.Count == 0)
                return NotFound();
            return Ok(feedbacks);
        }

        // GET api/feedback/{postId}
        [HttpGet("{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Feedback>>> GetByPostId(int postId)
        {
            var feedbacks = await _feedbackService.GetByPostId(postId);
                return NotFound();
            return Ok(feedbacks);
        }

        // POST api/feedback
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Feedback>> Create([FromBody] Feedback feedback)
        {
            if (feedback == null)
                return BadRequest("Invalid feedback data.");

            var result = await _feedbackService.Add(feedback);
            if (!result)
                return BadRequest("Failed to add feedback.");

            return CreatedAtAction(nameof(GetByPostId), new { postId = feedback.PostId }, feedback);
        }
    }
}
