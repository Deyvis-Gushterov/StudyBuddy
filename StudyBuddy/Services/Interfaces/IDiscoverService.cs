using StudyBuddy.ViewModels;

namespace StudyBuddy.Services.Interfaces
{
    public interface IDiscoverService
    {
        Task<DiscoverViewModel> GetSuggestionsAsync(string userId);
    }
}
