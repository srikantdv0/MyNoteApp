using System;
using NotesBlaze.Models;
using NotesShared.Models;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Timers;

namespace NotesBlaze.Services
{
    public class StateContainer
    {
        private INotesDataService _notesDataService;
        private IJSRuntime _jSRuntime;

        public StateContainer(INotesDataService notesDataService, IJSRuntime jSRuntime)
        {
            _notesDataService = notesDataService;
            _jSRuntime = jSRuntime;
        }
        public NoteId noteId { get; set; } = new NoteId();


        private List<NoteMetadata>? noteMetadata { get; set; }
        private List<SharedNoteMetadata>? sharedNoteMetadata { get; set; }
        private List<SharedNoteUsersDto>? sharedNoteUsersDto { get; set; }
        private ImageFile? profilePic;

        public event Action OnStateChange = null!;
        public event EventHandler<IEnumerable<NoteMetadata>?> metaDataEvent = null!;
        public event EventHandler<IEnumerable<SharedNoteMetadata>?> sharedmetaDataEvent = null!;
        public event EventHandler<IEnumerable<SharedNoteUsersDto>?> sharedNoteUsersEvent = null!;
        public event EventHandler<ImageFile?> profilePicEvent = default!;

        public event EventHandler userLoginEvent = null!;
        public event EventHandler userLogoutEvent = null!;

        public void SetValue(NoteId noteId)
        {
            this.noteId = noteId;
            NotifyStateChanged();
        }

        public async Task<List<NoteMetadata>?> GetNoteMetaData()
        {
            var res = await _notesDataService.GetNotes();
            if (res != null)
            {
                noteMetadata = res.ToList();
            }
            else
            {
                noteMetadata = null;
            }
            NoteMetaDataEvent();
            return noteMetadata;
        }

        public async Task<List<SharedNoteMetadata>?> GetSharedNoteMetaData()
        {
            var res = await _notesDataService.GetSharedNotes();
            if (res != null)
            {
                sharedNoteMetadata = res.ToList();
            }
            else
            {
                sharedNoteMetadata = null;
            }
            SharedNoteMetaDataEvent();
            return sharedNoteMetadata;
        }

        public async Task GetSharedNoteUsers()
        {
            if (noteId is not null)
            {
                var users = await _notesDataService.GetSharedNoteUsers(noteId.Id);
                if (users != null)
                {
                    sharedNoteUsersDto = users.ToList();
                }
                SharedNoteUsersEvent();
            }
        }

        public async Task<ImageFile?> GetProfilePic()
        {
            var res= await _notesDataService.GetProfilePic();
            if (res != null)
            {
                profilePic = res;
            }
            ProfilePicEvent();
            return profilePic;
        }

        public void ProfilePicEvent()
        {
            profilePicEvent?.Invoke(this,profilePic);
        }

        public void SharedNoteUsersEvent()
        {
            sharedNoteUsersEvent?.Invoke(this, sharedNoteUsersDto);
        }

        public void NoteMetaDataEvent()
        {
            metaDataEvent?.Invoke(this,noteMetadata);
        }

        public void SharedNoteMetaDataEvent()
        {
            sharedmetaDataEvent?.Invoke(this, sharedNoteMetadata);
        }

        public void NotifyStateChanged()
        {
            OnStateChange?.Invoke();
        }

        public async Task SaveUserSession(LoginModel loginModel,string token)
        {
            await _jSRuntime.InvokeVoidAsync("localStorage.setItem", "user", $"{loginModel.email};{token}").ConfigureAwait(false);
            userLoginEvent?.Invoke(this,EventArgs.Empty);
        }

        public async Task RemoveUserSession()
        {
            await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", "user").ConfigureAwait(false);
            userLogoutEvent?.Invoke(this,EventArgs.Empty);
        }

    }
}

