using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace DozukiSkillStationWebhook.Services
{
    public class DozukiAPI
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DozukiAPI> _logger;
        private readonly string _apiToken;
        private const string BaseUrl = "https://fike.dozuki.com/api/2.0";

        public DozukiAPI(IHttpClientFactory httpClientFactory, ILogger<DozukiAPI> logger, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _apiToken = configuration["DozukiAPI:Token"] ?? throw new InvalidOperationException("DozukiAPI:Token not configured");
        }

        public async Task<string?> CheckForQuiz(string title)
        {
            try
            {
                var encodedTitle = Uri.EscapeDataString(title);
                var requestUrl = $"{BaseUrl}/quizzes/sessions?query={encodedTitle}";
                
                _logger.LogInformation("Checking for Quiz with Title: {Title}", title);

                // Create request message to add custom headers
                using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Add("Authorization", "api " + _apiToken);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation("Quiz API response received for Title: {Title}", title);
                
                // Parse JSON to check if quizSessions array exists and is not empty
                using (JsonDocument doc = JsonDocument.Parse(content))
                {
                    if (doc.RootElement.TryGetProperty("quizSessions", out JsonElement quizSessionsElement) 
                        && quizSessionsElement.ValueKind == JsonValueKind.Array 
                        && quizSessionsElement.GetArrayLength() > 0)
                    {
                        _logger.LogInformation("Quiz found for Title: {Title}", title);
                        return content;
                    }
                    else
                    {
                        _logger.LogInformation("Quiz not found for Title: {Title}", title);
                        return null;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling Quiz API for Title: {Title}", title);
                throw;
            }
        }
    }
}
