using System;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using NotesBlaze.Models;
using NotesBlaze.Services;
using NotesBlaze.Components;
using Microsoft.JSInterop;
using Blazored.LocalStorage;

namespace NotesBlaze.Components
{
    public partial class Login : ComponentBase
    {
        [Inject]
        INotesDataService _notesDataService { get; set; } = default!;

        [Inject]
        NavigationManager _navigation { get; set; } = default!;

        [Inject]
        StateContainer _stateContainer { get; set; } = default!;


        LoginModel user = new LoginModel();
        bool isDisabled = false;
        public string message = "";

        private async Task OnValid()
        {
            isDisabled = true;

            var token = await _notesDataService.LoginAsync(user);
            if (!String.IsNullOrEmpty(token))
            {
                await _stateContainer.SaveUserSession(user,token);
                await _stateContainer.GetNoteMetaData();
                await _stateContainer.GetSharedNoteMetaData();
                _navigation.NavigateTo("/");
            }
            else {

                message = "Invalid credientials";
                isDisabled = false;
            }
                     
        }
    }
}

