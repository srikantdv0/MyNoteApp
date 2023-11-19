using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NotesBlaze.Models;
using NotesBlaze.Services;
using NotesShared.Models;
using static System.Net.WebRequestMethods;

namespace NotesBlaze.Components
{
    public partial class ShareWith : ComponentBase
    {

        [Inject]
        StateContainer stateContainer { get; set; } = default!;

        [Inject]
        INotesDataService notesDataSerive { get; set; } = default!;

        [Inject]
         IJSRuntime jsRuntime { get; set; } = default!;

        private List<UserProfileDto>? searchResult;
        private List<SharedNoteUsersDto>? sharedNoteUsersDto;
        private string? filter;
        private List<UserProfileDto>? usersMetaData;
        private ShareFormInput shareFormInput = new ShareFormInput();
        private string[] permissions = new string[2] {"Read","ReadWrite" };
        private string? userName;


        protected async override Task OnInitializedAsync()
        {
            var res = await notesDataSerive.GetUsersAsync();
            if (res != null)
            {
                usersMetaData = res.ToList();
            }

            stateContainer.sharedNoteUsersEvent += OnSharedNoteUserDataEventHandler;

            var userdata = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "user").ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(userdata))
            {
                userName = userdata.Split(';', 2)[0];
            }
        }

        private void OnSharedNoteUserDataEventHandler(object? sender, IEnumerable<SharedNoteUsersDto>? sharedNoteUsersDto)
        {
            if (sharedNoteUsersDto != null)
            {
                this.sharedNoteUsersDto = sharedNoteUsersDto.ToList();

            }
            else
            {
                this.sharedNoteUsersDto = null;
            }
            StateHasChanged();
        }

        async Task HandleInput(ChangeEventArgs e)
        {
            filter = e.Value?.ToString();
            if (filter?.Length > 2)
            {
                searchResult = await Task.FromResult(usersMetaData?.Where(a => a.Name.Contains(filter))
                    .Where(a => a.Email != userName).ToList());
            }
            else
            {
                searchResult = null;
                shareFormInput.selectedUserEmail = String.Empty;
            }
        }

        void SelectUser(UserProfileDto user)
        {
            shareFormInput.selectedUserEmail = user.Email;
            searchResult = null;
            shareFormInput.userProfileDto = user;
        }

        private async Task OnSubmit()
        {
            if (stateContainer.noteId.Id != 0)
            {
                var id = stateContainer.noteId.Id;
                await notesDataSerive.ShareNote(shareFormInput.userProfileDto.UserId, id, shareFormInput.permission);
                await stateContainer.GetSharedNoteUsers();
                shareFormInput = new ShareFormInput();
            }
        }

        private async Task UnShare(int userId)
        {
            if (stateContainer.noteId.Id != 0)
            {
                var id = stateContainer.noteId.Id;
                await notesDataSerive.UnshareNote(userId, id);
                await stateContainer.GetSharedNoteUsers();
            }
        }

        public class ShareFormInput
        {
            [Required]
            public string selectedUserEmail { get; set; } = String.Empty;

            [Required]
            public UserProfileDto userProfileDto { get; set; } = default!;

            [Required]
            public string permission { get; set; } = "Read";
        }
    }
}
