using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using practica.Models;
using practica.Data;  // Ajusta según tu namespace real del contexto

namespace practica.Service
{
    public class FeedbackService
    {
        private readonly ILogger<FeedbackService> _logger;
        private readonly ApplicationDbContext _context;

        public FeedbackService(ILogger<FeedbackService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<Feedback>?> GetAll()
        {
            _logger.LogInformation("Fetching all feedbacks from the database.");
            if (_context.DbSetFeedback == null)
                return null;

            var feedbacks = await _context.DbSetFeedback.ToListAsync();

            _logger.LogInformation("Fetched {Count} feedbacks from the database.", feedbacks.Count);
            return feedbacks;
        }

        public async Task<Feedback?> GetByPostId(int postId)
        {
            _logger.LogInformation("Fetching feedback for PostId {PostId} from the database.", postId);
            if (_context.DbSetFeedback == null)
                return null;

            var feedback = await _context.DbSetFeedback.FirstOrDefaultAsync(f => f.PostId == postId);
            if (feedback == null)
            {
                _logger.LogInformation("No feedback found for PostId {PostId}.", postId);
                return null;
            }
            _logger.LogInformation("Fetched feedback for PostId {PostId} from the database.", postId);
            return feedback;
        }

        public async Task<bool> Add(Feedback feedback)
        {
            _logger.LogInformation("Adding a new feedback for PostId {PostId} to the database.", feedback.PostId);
            if (_context.DbSetFeedback == null)
                return false;

            await _context.DbSetFeedback.AddAsync(feedback);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Added new feedback for PostId {PostId} to the database.", feedback.PostId);
            return true;
        }

        public async Task<bool> Update(Feedback feedback)
        {
            _logger.LogInformation("Updating feedback for PostId {PostId} in the database.", feedback.PostId);
            if (_context.DbSetFeedback == null)
                return false;

            _context.Entry(feedback).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated feedback for PostId {PostId} in the database.", feedback.PostId);
            return true;
        }
public async Task<bool> FeedbackExistsAsync(int postId)
{
    return await _context.DbSetFeedback.AnyAsync(f => f.PostId == postId);
}


    }
}
