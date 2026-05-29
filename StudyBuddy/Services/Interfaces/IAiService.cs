namespace StudyBuddy.Services.Interfaces
{
    public interface IAiService
    {

        Task<string> GetStudyAssistantResponseAsync(string noteContent, string userQuestion);


        Task<string> SummarizeNoteAsync(string noteContent);

        Task<string> GetWritingSuggestionsAsync(string blogContent);

        Task<List<string>> SuggestTagsAsync(string blogContent);

        Task<string> ChatAsync(List<(string role, string content)> history, string userMessage);
    }
}
