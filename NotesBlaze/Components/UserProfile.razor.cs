using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NotesBlaze.Services;
using NotesShared.Models;
using static System.Net.WebRequestMethods;

namespace NotesBlaze.Components
{
    public partial class UserProfile : ComponentBase
    {
        [Inject]
        INotesDataService notesDataService { get; set; } = default!;

        [Inject]
        StateContainer stateContainer { get; set; } = default!;

        ImageFile filesBase64 = new ImageFile();
        string message = String.Empty;
        bool isDisabled = false;
        ImageFile profilePic = default!;
        UserProfileDto user = default!;

        protected override async Task OnInitializedAsync()
        {
            var res =  await stateContainer.GetProfilePic();
            if (res != null)
            {
                profilePic = res;
            }
            var userDetails = await notesDataService.UserProfileAsync();

            if (userDetails != null)
            {
                user = userDetails;
            }
        }

        async Task OnChange(InputFileChangeEventArgs e)
        {
            message = String.Empty;
            var files = e.GetMultipleFiles(); // get the files selected by the users
            foreach (var file in files)
            {
                var resizedFile = await file.RequestImageFileAsync(file.ContentType, 600, 480); // resize the image file
                var buf = new byte[resizedFile.Size]; // allocate a buffer to fill with the file's data
                using (var stream = resizedFile.OpenReadStream())
                {
                    await stream.ReadAsync(buf); // copy the stream to the buffer
                }
                filesBase64 = new ImageFile { Base64data = Convert.ToBase64String(buf), ContentType = file.ContentType, FileName = file.Name }; // convert to a base64 string!!p
            }
        }

        async Task Upload()
        {
            isDisabled = true;
            if (filesBase64.Base64data != String.Empty)
            {
                var res = await notesDataService.UploadProfilePic(filesBase64);
                message = res;
                await stateContainer.GetProfilePic();
            }
            isDisabled = false;
        }
    }
}

