using System.Net.Http;
using System.Text.Json;

namespace practica.Integration.Exchange
{
    public class PostIntegration
    {
        private readonly ILogger<PostIntegration> _logger;

        public PostIntegration(ILogger<PostIntegration> logger)
        {
            _logger = logger;
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            var url = "https://jsonplaceholder.typicode.com/posts";
            using var httpClient = new HttpClient();

            try
            {
                _logger.LogInformation("Fetching posts from {Url}", url);
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var posts = JsonSerializer.Deserialize<List<Post>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation("Successfully fetched {Count} posts", posts?.Count ?? 0);
                    return posts ?? new List<Post>();
                }
                else
                {
                    _logger.LogError("Failed to fetch posts. Status code: {StatusCode}", response.StatusCode);
                    return new List<Post>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching posts");
                return new List<Post>();
            }
        }
    }
}
