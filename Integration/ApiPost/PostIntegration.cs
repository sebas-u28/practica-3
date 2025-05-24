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

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var url = $"https://jsonplaceholder.typicode.com/users/{id}";
            using var httpClient = new HttpClient();

            try
            {
                _logger.LogInformation("Fetching user from {Url}", url);
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation("Successfully fetched user with ID {Id}", id);
                    return user;
                }
                else
                {
                    _logger.LogError("Failed to fetch user. Status code: {StatusCode}", response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching user");
                return null;
            }
        }
        public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            var url = $"https://jsonplaceholder.typicode.com/comments?postId={postId}";
            using var httpClient = new HttpClient();

            try
            {
                _logger.LogInformation("Fetching comments for post ID {PostId} from {Url}", postId, url);
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var comments = JsonSerializer.Deserialize<List<Comment>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation("Fetched {Count} comments for post ID {PostId}", comments?.Count ?? 0, postId);
                    return comments ?? new List<Comment>();
                }
                else
                {
                    _logger.LogError("Failed to fetch comments. Status code: {StatusCode}", response.StatusCode);
                    return new List<Comment>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching comments for post ID {PostId}", postId);
                return new List<Comment>();
            }
        }

    }
}
