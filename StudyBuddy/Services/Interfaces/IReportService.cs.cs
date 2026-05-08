using StudyBuddy.Models;

namespace StudyBuddy.Services.Interfaces
{
    public interface IReportService
    {
        Task<Report?> GetReportByIdAsync(int id);
        Task<List<Report>> GetAllReportsAsync();
        Task<Report?> CreateReportAsync(Report report);
        Task<bool> DeleteReportAsync(int id);
        Task<Report?> GetReportWithOwnerAsync(int id);
        Task<bool> ResolveReportAsync(int id);

    }
}
