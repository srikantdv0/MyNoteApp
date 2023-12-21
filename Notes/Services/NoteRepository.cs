using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Notes.DbContexts;
using Notes.Entities;
using NotesShared.Models;

namespace Notes.Services
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteContext _context;

        public NoteRepository(NoteContext noteContext)
        {
            _context = noteContext ?? throw new ArgumentNullException(nameof(noteContext));
        }

        public async Task<int> CreateNoteAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
            return note.Id;
        }

        public void DeleteNote(Note note)
        {
            _context.Notes.Remove(note);
        }

        public async Task<Note?> GetNoteAsync(int noteId, int? userContextId)
        {
            IQueryable<Note> note =  _context.Notes
                    .Where(n => n.Id == noteId);

            if (userContextId != null)
            {
                note = note.Where(n => n.CreatorId == userContextId);
            }
            return await note.FirstOrDefaultAsync();
            
        }

        public async Task<int?> GetNoteCreatorAsync(int noteId)
        {
            return await _context.Notes
                .Where(n => n.Id == noteId)
                .Select(n => n.CreatorId)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Note?>> GetNotesAsync(int? userContextId)
        {
            IQueryable<Note> notes = _context.Notes;
            if (userContextId != null)
            {
               notes = notes.Where(n => n.CreatorId == userContextId);
            }
            return await notes.OrderByDescending(a => a.CreatedDTS).ToListAsync();
        }

        public async Task<IEnumerable<SharedNote?>> GetSharedNoteDetailsAsync(int noteId)
        {
           return await _context.SharedNotes
                .Include(a => a.User)
                .Include(a => a.Permission)
                .Where(a => a.NoteId == noteId).ToListAsync();
        }

        public async Task<Note?> GetSharedNoteAsync(int noteId, int? userContextId)
        {
            IQueryable<Note> note = _context.Notes 
                      .Where(n => n.Id == noteId && n.SharedNote.Any());
            if (userContextId != null)
            {
                note = note.Include(i => i.SharedNote.Where(a => a.SharedToId == userContextId));
                note = note.Where(i => i.CreatorId != userContextId);
            }
            else
            {
                note = note.Include(i => i.SharedNote);
            }          

            return await note.FirstOrDefaultAsync();
        }

        public IEnumerable<Note?> GetSharedNotesAsync(int? userContextId)
        {
            IQueryable<Note> notes = _context.Notes
                       .Where(i => i.SharedNote.Any()); ;
            if (userContextId != null)
            {
                notes = notes.Include(a => a.SharedNote.Where(i => i.SharedToId == userContextId));
                notes = notes.Where(i => i.CreatorId != userContextId);
            }
            else
            {
                notes = notes.Include(i => i.SharedNote);
            }
            List<Note?> result = new List<Note?>();
            foreach (var note in notes)
            {
                if (note.SharedNote.Where(i => i.SharedToId == userContextId).Count() > 0)
                {
                    result.Add(note);
                }
            }
            return result;
        }

        public async Task<SharedNote?> GetSharedNoteToAsync(int userId, int noteId)
        {
            return await _context.SharedNotes
                .Where(a => a.NoteId == noteId && a.SharedToId == userId)
                .FirstOrDefaultAsync();

        }

        public async Task<string?> GetSharedPermissionAsync(int noteId, int userId)
        {
            var permission = await GetSharedNoteToAsync(userId, noteId);
            if (permission == null)
            {
                return null;
            }
                return await _context.Permissions          
                    .Where(a => a.Id == permission.PermissionId)
                    .Select(a => a.Description).FirstOrDefaultAsync();
            
        }

        public async Task<User?> IsValidAsync(string email, string password)
        {
            var res = await _context.Users
                .Where(a => a.Email == email
                && string.Equals(a.Password,password)
                && a.IsActive)
                .FirstOrDefaultAsync();

            return res;
        }

        public async Task<IEnumerable<Permission>> LoadSharedPermissionMetadata()
        {
            return await _context.Permissions
                   .ToListAsync();         
                
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task ShareNoteAsync(int userId, int noteId, int permissionId)
        {
            var permission = await GetSharedNoteToAsync(userId, noteId);
            if (permission == null)
            {
                await _context.AddAsync(
                    new SharedNote
                    {
                        SharedToId = userId,
                        NoteId = noteId,
                        PermissionId = permissionId
                    }
                    );
            }
            else
            {
                 permission.PermissionId = permissionId;
            }
          
        }

        public void UnShareNote(SharedNote sharedNote)
        {
            _context.SharedNotes.Remove(sharedNote);
                
        }

        public void UpdateNote(Note noteforUpdate)
        {
             _context.Notes.Update(noteforUpdate);
        }

    }
}