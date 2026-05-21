using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Services.Implementations
{
    public class StudyGroupService: IStudyGroupService
    {
        
        private readonly ApplicationDbContext context;

        public StudyGroupService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<StudyGroup>> GetAllGroupsAsync()
        {
            return await context.StudyGroups
                .Include(g => g.Creator)
                .ToListAsync();
        }

        public async Task<List<StudyGroup>> GetGroupByCreatorIdAsync(string creatorId)
        {
            return await context.StudyGroups
                .Include(g => g.Creator)
                .Where(g => g.CreatorId == creatorId)
                .ToListAsync();
        }

        public async Task<StudyGroup?> GetByIdAsync(int id)
        {
            return await context.StudyGroups
                .Include(g => g.Creator)
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<List<StudyGroup>> GetUserGroupsAsync(string userId)
        {
            return await context.StudyGroupMembers
                .Where(m => m.UserId == userId)
                .Include(m => m.StudyGroup)
                .Select(m => m.StudyGroup)
                .ToListAsync();
        }
        public async Task<StudyGroup?> CreateGroupAsync(StudyGroup group, string creatorId)
        {
            if (group == null)
            {
                return null;
            }

            var creator = await context.Users.FirstOrDefaultAsync(u => u.Id == creatorId);

            if (creator == null)
            {
                return null;
            }

            group.Creator = creator;
            context.StudyGroups.Add(group);
            await context.SaveChangesAsync(); 

            var creatorMember = new StudyGroupMember
            {
                StudyGroupId = group.Id, 
                UserId = creatorId,
                JoinedAt = DateTime.UtcNow,
                Role = Role.Admin
            };

            context.StudyGroupMembers.Add(creatorMember);
            await context.SaveChangesAsync();
            return group;
        }
        public async Task<bool> JoinGroupAsync(int groupId, string userId)
        {
            var alreadyMember = await context.StudyGroupMembers
                .AnyAsync(m => m.StudyGroupId == groupId && m.UserId == userId);

            if (alreadyMember) return false;

            var member = new StudyGroupMember
            {
                StudyGroupId = groupId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow,
                Role = Role.Member
            };

            context.StudyGroupMembers.Add(member);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> LeaveGroupAsync(int groupId, string userId)
        {
            var member = await context.StudyGroupMembers
                .FirstOrDefaultAsync(m => m.StudyGroupId == groupId && m.UserId == userId);

            if (member == null) return false;

            context.StudyGroupMembers.Remove(member);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUserMemberAsync(int groupId, string userId)
        {
            return await context.StudyGroupMembers
                .AnyAsync(m => m.StudyGroupId == groupId && m.UserId == userId);
        }

    }
}
