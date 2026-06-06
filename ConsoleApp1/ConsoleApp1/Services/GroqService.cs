using Microsoft.Extensions.Configuration;

namespace StudyBuddyBot.Services
{
    public class GroqService
    {
        private readonly string apiKey;
        private readonly string model;
        private readonly HttpClient httpClient;

        public GroqService(IConfiguration configuration)
        {
            this.apiKey = configuration["Groq:ApiKey"]!;
            this.model = configuration["Groq:Model"]!;
            this.httpClient = new HttpClient();
        }

        public async Task<string> AskAsync(string prompt)
        {
            var url = "https://api.groq.com/openai/v1/chat/completions";

            var requestBody = new
            {
                model = this.model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = await httpClient.PostAsync(url, content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return $"Error: {responseJson}";

            using var doc = System.Text.Json.JsonDocument.Parse(responseJson);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No response.";
        }
    }
}