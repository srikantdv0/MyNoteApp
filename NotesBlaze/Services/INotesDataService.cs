using System;
using System.Threading.Tasks;
using NotesBlaze.Models;
using NotesShared.Models;

namespace NotesBlaze.Services
{
    public interface INotesDataService
    {
        Task<string?> LoginAsync(LoginModel login);
        Task<string> LogoutAsync();
        Task<IEnumerable<NoteMetadata>?> GetNotes();
        Task<IEnumerable<SharedNoteMetadata>?> GetSharedNotes();
        Task<NoteMetadata?> GetNotes(int Id);
        Task<SharedNoteMetadata?> GetSharedNotes(int Id);
        Task<NoteContentDto?> GetNote(int id);
        Task<string?> UpdateNote(int id, NoteContentDto noteforUpdate);
        Task<int?> CreateNote(NotesForCreationDto noteForCreation);
        Task<string?> DeleteNote(int id);
        Task<(string Message, UserProfileDto? UserProfile)> UserProfileAsync();
        Task<IEnumerable<UserProfileDto>?> GetUsersAsync();
        Task<string> RegisterAsync(UserForCreationDto userForCreationDto);
        Task<string> SendOTPAsync(ConfirmOTP confirmOTP);
        Task<bool> VerifyOTPAsync(ConfirmOTP confirmOTP);
        Task<string> ForgetPassword(ConfirmOTP confirmOTP, ResetPassword resetPassword);
        Task<string?> ShareNote(int userId, int noteId, string permission);
        Task<SharedNoteContentDto?> GetSharedNote(int id);
        Task<string?> UnSubscribeToNote(int Id);
        Task<IEnumerable<SharedNoteUsersDto>?> GetSharedNoteUsers(int NoteId);
        Task<string?> UnshareNote(int userId, int noteId);
        Task<string> UploadProfilePic(ImageFile imageFiles);
        Task<ImageFile?> GetProfilePic();
    }
}

