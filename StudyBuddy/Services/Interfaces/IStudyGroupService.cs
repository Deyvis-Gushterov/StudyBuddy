using StudyBuddy.Models;

namespace StudyBuddy.Services.Interfaces
{
    public interface IStudyGroupService
    {
        Task<List<StudyGroup>> GetAllGroupsAsync();
        Task<StudyGroup?> GetByIdAsync(int id);
        Task<List<StudyGroup>> GetUserGroupsAsync(string userId);
        Task<StudyGroup?> CreateGroupAsync(StudyGroup group, string creatorId);
        Task<bool> JoinGroupAsync(int groupId, string userId);
        Task<bool> LeaveGroupAsync(int groupId, string userId);
        Task<bool> IsUserMemberAsync(int groupId, string userId);
        Task<List<StudyGroup>> GetGroupByCreatorIdAsync(string creatorId);
    }
}
