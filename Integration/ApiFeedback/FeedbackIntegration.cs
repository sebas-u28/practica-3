using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using practica.Models;
using practica.Service;

namespace practica.Integration
{
    public class FeedbackIntegration
    {
        private readonly ILogger<FeedbackIntegration> _logger;
        private readonly FeedbackService _feedbackService;

        public FeedbackIntegration(ILogger<FeedbackIntegration> logger, FeedbackService feedbackService)
        {
            _logger = logger;
            _feedbackService = feedbackService;
        }

        // Obtener todos los feedbacks
        public async Task<List<Feedback>> GetAllFeedbacksAsync()
        {
            try
            {
                _logger.LogInformation("Fetching feedbacks from service...");
                var feedbacks = await _feedbackService.GetAll();

                _logger.LogInformation("Feedbacks retrieved: {Count}", feedbacks?.Count ?? 0);
                return feedbacks ?? new List<Feedback>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching feedbacks");
                return new List<Feedback>();
            }
        }

        // Crear un nuevo feedback
        public async Task<bool> PostFeedbackAsync(Feedback feedback)
        {
            try
            {
                _logger.LogInformation("Adding feedback for PostId {PostId} with Sentimiento {Sentimiento}", feedback.PostId, feedback.Sentimiento);
                var success = await _feedbackService.Add(feedback);

                if (success)
                {
                    _logger.LogInformation("Feedback added successfully");
                }
                else
                {
                    _logger.LogError("Failed to add feedback");
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while adding feedback");
                return false;
            }
        }
    }
}
