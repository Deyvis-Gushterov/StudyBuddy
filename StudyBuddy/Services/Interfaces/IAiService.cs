namespace StudyBuddy.Services.Interfaces
{
    public interface IAiService
    {

        Task<string> GetStudyAssistantResponseAsync(string noteContent, string userQuestion);


        Task<string> SummarizeNoteAsync(string noteContent);

        Task<string> GetWritingSuggestionsAsync(string blogContent);


        Task<List<string>> SuggestTagsAsync(string blogContent);
    }
}
