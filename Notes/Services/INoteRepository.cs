using System;
using Notes.Entities;
using NotesShared.Models;

namespace Notes.Services
{
	public interface INoteRepository
	{
		Task<int> CreateNoteAsync(Note note);
		void UpdateNote(Note noteforUpdate);
		void DeleteNote(Note note);

		Task<bool> SaveChangesAsync();

        Task<Note?> GetNoteAsync(int noteId, int? userContextId);
        Task<IEnumerable<Note?>> GetNotesAsync(int? userContextId);
        Task<Note?> GetSharedNoteAsync(int noteId, int? userContextId);
        IEnumerable<Note?> GetSharedNotesAsync(int? userContextId);

        Task<SharedNote?> GetSharedNoteToAsync(int userId, int noteId);

        Task ShareNoteAsync(int userId, int noteId, int permissionId);
		void UnShareNote(SharedNote sharedNote);

        Task<int?> GetNoteCreatorAsync(int noteId);
        Task<IEnumerable<Permission>> LoadSharedPermissionMetadata();
        Task<string?> GetSharedPermissionAsync(int noteId, int userId);

        Task<User?> IsValidAsync(string email,string password);

        Task<IEnumerable<SharedNote?>> GetSharedNoteDetailsAsync(int noteId);
    }
}

