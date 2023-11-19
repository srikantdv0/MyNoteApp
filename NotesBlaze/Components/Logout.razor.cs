using System;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using NotesBlaze.Models;
using NotesBlaze.Services;
using NotesBlaze.Components;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using NotesShared.Models;

namespace NotesBlaze.Components
{
    public partial class Logout : ComponentBase
    {

        [Inject]
        StateContainer _stateContainer { get; set; } = default!;

        protected async override Task OnInitializedAsync()
        {
            await _stateContainer.RemoveUserSession();
        }
        
    }
}

