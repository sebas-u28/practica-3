using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using practica.Models;
using practica.Service;

namespace practica.Integration
{
    public class FeedbackIntegration
    {
        private readonly ILogger<FeedbackIntegration> _logger;
        private readonly HttpClient _httpClient;
        private readonly FeedbackService _feedbackService; // 👈 nueva dependencia


        public FeedbackIntegration(
            ILogger<FeedbackIntegration> logger,
            HttpClient httpClient,
            FeedbackService feedbackService) // 👈 inyéctalo aquí
        {
            _logger = logger;
            _httpClient = httpClient;
            _feedbackService = feedbackService;
        }

        // Obtener todos los feedbacks (GET api/feedback)
        public async Task<List<Feedback>> GetAllFeedbacksAsync()
        {
            try
            {
                _logger.LogInformation("Fetching feedbacks from API...");
                var response = await _httpClient.GetAsync("api/feedback");

                if (response.IsSuccessStatusCode)
                {
                    var feedbacks = await response.Content.ReadFromJsonAsync<List<Feedback>>();
                    _logger.LogInformation("Feedbacks retrieved: {Count}", feedbacks?.Count ?? 0);
                    return feedbacks ?? new List<Feedback>();
                }
                else
                {
                    _logger.LogError("Failed to fetch feedbacks. Status code: {StatusCode}", response.StatusCode);
                    return new List<Feedback>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching feedbacks");
                return new List<Feedback>();
            }
        }

        // Crear un nuevo feedback (POST api/feedback)
        public async Task<bool> PostFeedbackAsync(Feedback feedback)
        {
            try
            {
                _logger.LogInformation("Posting feedback for PostId {PostId} with Sentimiento {Sentimiento}", feedback.PostId, feedback.Sentimiento);
                var response = await _httpClient.PostAsJsonAsync("api/feedback", feedback);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Feedback posted successfully");
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to post feedback. Status code: {StatusCode}", response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while posting feedback");
                return false;
            }
        }
        public async Task<bool> FeedbackExistsAsync(int postId)
        {
            return await _feedbackService.FeedbackExistsAsync(postId);
        }

    }
}
