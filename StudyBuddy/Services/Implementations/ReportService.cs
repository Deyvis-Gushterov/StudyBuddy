using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;


namespace StudyBuddy.Services.Implementations
{
    public class ReportService: IReportService
    {
        private readonly ApplicationDbContext context;

        public ReportService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Report?> GetReportByIdAsync(int id)
        {
            var report = await context.Reports
                .Include(r => r.Reporter)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            if (report == null)
            {
                return null;
            }

            return report;
        }
        public async Task<List<Report>> GetAllReportsAsync()
        {
            return await context.Reports
                .Include (r => r.Reporter)
                .ToListAsync();
                
        }
        public async Task<Report?> CreateReportAsync(Report report)
        {
            if (report == null)
            {
                return null;
            }

            context.Reports.Add(report);
            await context.SaveChangesAsync();
            return report;
        }
        public async Task<bool> DeleteReportAsync(int id)
        {
            var report = await context.Reports
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            if (report == null)
            {
                return false;
            }

            context.Reports.Remove(report);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<Report?> GetReportWithOwnerAsync(int id)
        {
            var report = await context.Reports
        .Include(r => r.Reporter)
        .Include(r => r.Blog)
        .Include(r => r.Note)
        .Include(r => r.Comment)
        .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null)
            {
                return null;
            }

            return report;
        }
        public async Task<bool> ResolveReportAsync(int id)
        {
            var report = await context.Reports
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            if (report == null)
            {
                return false;
            }

            report.IsResolved = true;
            await context.SaveChangesAsync();
            return true;

        }
    }
}
