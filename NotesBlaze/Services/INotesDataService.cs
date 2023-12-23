using System;
using System.Threading.Tasks;
using NotesBlaze.Models;
using NotesShared.Models;

namespace NotesBlaze.Services
{
    public interface INotesDataService
    {
        Task<string?> LoginAsync(LoginModel login);
        Task LogoutAsync();
        Task<IEnumerable<NoteMetadata>?> GetNotes();
        Task<IEnumerable<SharedNoteMetadata>?> GetSharedNotes();
        Task<NoteMetadata?> GetNotes(int Id);
        Task<SharedNoteMetadata?> GetSharedNotes(int Id);
        Task<NoteContentDto?> GetNote(int id);
        Task UpdateNote(int id, NoteContentDto noteforUpdate);
        Task<int?> CreateNote(NotesForCreationDto noteForCreation);
        Task DeleteNote(int id);
        Task<UserProfileDto?> UserProfileAsync();
        Task<IEnumerable<UserProfileDto>?> GetUsersAsync();
        Task<string> RegisterAsync(UserForCreationDto userForCreationDto);
        Task<string> SendOTPAsync(ConfirmOTP confirmOTP);
        Task<bool> VerifyOTPAsync(ConfirmOTP confirmOTP);
        Task<string> ForgetPassword(ConfirmOTP confirmOTP, ResetPassword resetPassword);
        Task ShareNote(int userId, int noteId, string permission);
        Task<SharedNoteContentDto?> GetSharedNote(int id);
        Task UnSubscribeToNote(int Id);
        Task<IEnumerable<SharedNoteUsersDto>?> GetSharedNoteUsers(int NoteId);
        Task UnshareNote(int userId, int noteId);
        Task<string> UploadProfilePic(ImageFile imageFiles);
        Task<ImageFile?> GetProfilePic();
    }
}

