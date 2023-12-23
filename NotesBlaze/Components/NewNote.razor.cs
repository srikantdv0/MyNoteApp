using System;
using Microsoft.AspNetCore.Components;
using NotesBlaze.Models;
using NotesBlaze.Services;
using NotesShared.Models;

namespace NotesBlaze.Components
{
    public partial class NewNote : ComponentBase
    {
        [Inject]
        INotesDataService notesDataService { get; set; } = default!;

        [Inject]
        NavigationManager navigation { get; set; } = default!;

        [Inject]
        StateContainer stateContainer { get; set; } = default!;

        private NotesForCreationDto notesForCreationDto = new NotesForCreationDto();
        private bool isDisabled;

        private async Task Submit()
        {
            isDisabled = true;
            var id = await notesDataService.CreateNote(notesForCreationDto);
            if (id is null)
            {
                isDisabled = false;
                return;
            }
            await stateContainer.GetNoteMetaData();
            stateContainer.SetValue(new NoteId { Id = (int)id });
            navigation.NavigateTo("/note");
            isDisabled = false;
        }
    }
}

