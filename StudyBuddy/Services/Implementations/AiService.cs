using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Services.Implementations
{
    public class AiService : IAiService
    {
        private readonly string apiKey;
        private readonly string model;
        private readonly HttpClient httpClient;

        public AiService(IConfiguration configuration, HttpClient httpClient)
        {
            this.apiKey = configuration["Groq:ApiKey"]!;
            this.model = configuration["Groq:Model"]!;
            this.httpClient = httpClient;
        }

        private async Task<string> CallAiAsync(string prompt)
        {
            if (string.IsNullOrEmpty(apiKey))
                return "API key is empty — check User Secrets!";

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
                return $"API Error {response.StatusCode}: {responseJson}";

            using var doc = System.Text.Json.JsonDocument.Parse(responseJson);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No response received.";
        }

        private async Task<string> CallAiWithMessagesAsync(object[] messages)
        {
            if (string.IsNullOrEmpty(apiKey))
                return "API key is empty — check User Secrets!";

            var url = "https://api.groq.com/openai/v1/chat/completions";

            var requestBody = new { model = this.model, messages, max_tokens = 1024 };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = await httpClient.PostAsync(url, content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return $"API Error {response.StatusCode}: {responseJson}";

            using var doc = System.Text.Json.JsonDocument.Parse(responseJson);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No response received.";
        }

        public async Task<string> ChatAsync(List<(string role, string content)> history, string userMessage)
        {
            var systemMessage = new { role = "system", content = "You are StudyBuddy AI, a friendly and knowledgeable study assistant for students. Help with studying, explain concepts, suggest study strategies, and answer academic questions across all subjects. Keep responses concise and student-friendly." };

            var messages = new List<object> { systemMessage };

            foreach (var (role, content) in history)
                messages.Add(new { role, content });

            messages.Add(new { role = "user", content = userMessage });

            return await CallAiWithMessagesAsync(messages.ToArray());
        }

        public async Task<string> GetStudyAssistantResponseAsync(string noteContent, string userQuestion)
        {
            var prompt = $"""
        You are a helpful study assistant. A student is studying the following notes:

        {noteContent}

        The student asks: {userQuestion}

        Answer clearly and concisely based on the notes provided. 
        If the answer isn't in the notes, say so and provide general knowledge instead.
        """;

            return await CallAiAsync(prompt);
        }

        public async Task<string> SummarizeNoteAsync(string noteContent)
        {
            var prompt = $"""
        You are a helpful study assistant. Summarize the following study notes 
           in 10-12 clear and concise sentences:

           {noteContent}
        """;

            return await CallAiAsync(prompt);
        }

        public async Task<string> GetWritingSuggestionsAsync(string blogContent)
        {
            var prompt = $"""
        You are a helpful study assistant. A student is creating a blog:

        {blogContent}

        The student asks you for suggestions in writing that blog: style, word choice, terminology

        Answer clearly and concisely based on the provided blog. 
        You are can also use general knowledge.
        """;

            return await CallAiAsync(prompt);
        }

        public async Task<List<string>> SuggestTagsAsync(string blogContent)
        {
            var prompt = $"""
        You are a helpful study assistant. A student is creating a blog:

        {blogContent}

        Suggest exactly 5 relevant tags for this blog.
        Respond with ONLY a comma-separated list, nothing else.
        Example: Mathematics, Calculus, Integration, University, Study Guide
        """;

            var result = await CallAiAsync(prompt);

            return result
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .Take(5)
                .ToList();
        }
    }
}
