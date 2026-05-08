using Microsoft.EntityFrameworkCore;
using StudyBuddy.Data;
using StudyBuddy.Models;
using StudyBuddy.Services.Interfaces;


namespace StudyBuddy.Services.Implementations
{
    public class NoteService: INoteService
    {
        private readonly ApplicationDbContext context;

        public NoteService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Note?> GetNoteByIdAsync(int id)
        {
            return await context.Notes
                .Where(n => n.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Note>> GetAllNotesAsync()
        {

            return await context.Notes
                .Include(n => n.Creator)
                .ToListAsync();
        }

        public async Task<Note> CreateNoteAsync(Note note)
        {
            if (note == null)
            {
                return null;
            }

            if (note.Creator == null)
            {
                return null;
            }

            context.Notes.Add(note);
            await context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateNoteAsync(int id, Note note)
        {
            if (note == null)
            {
                return null;
            }
            var oldNote = await context.Notes
                .Where(n => n.Id == id)
                .FirstOrDefaultAsync();

            if (oldNote == null)
            {
                return null;
            }

            oldNote.Topic = note.Topic;
            oldNote.Subject = note.Subject;
            oldNote.Content = note.Content;
            oldNote.BriefExplanation = note.BriefExplanation;

            await context.SaveChangesAsync();
            return oldNote;

        }

        public async Task<bool> DeleteNoteAsync(int id)
        {
            var note = await context.Notes
                .Where(n => n.Id == id)
                .FirstOrDefaultAsync();
                
            if (note == null)
            {
                return false;
            }

            context.Notes.Remove(note);
            await context.SaveChangesAsync();
            return true;
            
        }

        public async Task<Note> GetNoteWithOwnerAsync(int id)
        {
            var note = await context.Notes
                .Include(n => n.Creator)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                return null;
            }

            return note;
        }

        public async Task<bool> LikeAsync(int id)
        {
            var note = await context.Notes
                .Where(n => n.Id == id)
                .FirstOrDefaultAsync();

            if(note == null)
            {
                return false;
            }

            note.Likes++;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
