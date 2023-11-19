using System;
using Microsoft.AspNetCore.Components;
using NotesBlaze.Models;
using NotesBlaze.Services;
using NotesShared.Models;
using static System.Net.WebRequestMethods;

namespace NotesBlaze.Components
{
    public partial class AutoComplete : ComponentBase
    {
        [Parameter]
        public List<NoteMetadata>? noteMetadata { get; set; }

        [Inject]
        StateContainer? stateContainer { get; set; }

        [Inject]
        NavigationManager? navigationManager { get; set; }

        List<NoteMetadata>? searchResult;
        string? selectedNoteName;
        string? filter;


        async Task HandleInput(ChangeEventArgs e)
        {
            filter = e.Value?.ToString();
            if (filter?.Length > 2)
            {
                searchResult = await Task.FromResult(noteMetadata?.Where(a => a.Title.Contains(filter)).ToList());
            }
            else
            {
                searchResult = null;
                
               selectedNoteName = null;
            }
        }

        void SelectNote(int id)
        {
            stateContainer?.SetValue(new NoteId { Id = id });
            navigationManager?.NavigateTo("/note");
            searchResult = null;
            filter = null;
            selectedNoteName = null;
        }
    }
}

