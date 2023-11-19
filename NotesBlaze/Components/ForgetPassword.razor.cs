using System;
using System.Diagnostics.Metrics;
using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NotesBlaze.Models;
using NotesBlaze.Services;
using NotesShared.Models;
using static System.Net.WebRequestMethods;

namespace NotesBlaze.Components
{
    public partial class ForgetPassword : ComponentBase, IDisposable
    {

        [Inject]
        INotesDataService _notesDataService { get; set; } = default!;

        [Inject]
        NavigationManager _navigationManager { get; set; } = default!;

        ConfirmOTP confirmOTP = new ConfirmOTP();
        bool isDisabled = true;
        bool isSendOTPDisabled = false;
        bool isResetPasswordVisible = false;
        bool isGenerateOTPVisible = true;
        string message = String.Empty;
        string resendIn = String.Empty;

        private static System.Timers.Timer aTimer = default!;
        private int counter;

        private async Task OnValid()
        {
            var res = await _notesDataService.VerifyOTPAsync(confirmOTP);
            if (!res)
            {
                message = "Incorrect OTP";
            }
            else
            {
                isGenerateOTPVisible = false;
                isResetPasswordVisible = true;
                StateHasChanged();
            }
        }

        private async Task SendOTP()
        {
            isSendOTPDisabled = true;
            counter = 30;
            var res = await _notesDataService.SendOTPAsync(confirmOTP);
            message = res;
            if (String.Equals(res, "OTP sent to your email address"))
            {
                isDisabled = false;
                aTimer = new System.Timers.Timer();
                aTimer.Interval = 1000;
                aTimer.Elapsed += CountDownTimer;
                aTimer.Start();
            }
            else
            {
                isSendOTPDisabled = false;
            }
        }

        public void CountDownTimer(Object? source, System.Timers.ElapsedEventArgs e)
        {
            if (counter > 0)
            {
                resendIn = $"Resend OTP in {counter} seconds";
                counter -= 1;
            }
            else
            {
                aTimer.Stop();
                aTimer.Dispose();
                resendIn = String.Empty;
                isSendOTPDisabled = false;
                aTimer.Enabled = false;
            }
            StateHasChanged();
        }

        public void Dispose()
        {
            if (aTimer is not null)
            {
                aTimer.Dispose();
            }
        }
    }
}

