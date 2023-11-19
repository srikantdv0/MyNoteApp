using System;
using Microsoft.AspNetCore.Components;
using NotesBlaze.Services;
using NotesShared.Models;

namespace NotesBlaze.Components
{
    public partial class ResetPassword : ComponentBase
    {
        [Inject]
        INotesDataService notesDataService { get; set; } = default!;

        [Parameter]
        public ConfirmOTP confirmOtp { get; set; } = default!;

        private NotesBlaze.Models.ResetPassword resetPassword = new NotesBlaze.Models.ResetPassword();
        private string message = String.Empty;

        private async Task OnValid()
        {
            var res = await notesDataService.ForgetPassword(confirmOtp, resetPassword) ;
            message = res;
        }
    }
}

