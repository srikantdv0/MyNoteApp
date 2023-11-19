using System;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using NotesShared.Models;
using NotesBlaze.Services;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace NotesBlaze.Components
{
    
    public partial class NoteContent : ComponentBase, IDisposable
    {

        [Inject]
        INotesDataService notesDataService { get; set; } = null!;

        [Inject]
        NavigationManager navigation { get; set; } = null!;

        [Inject]
        StateContainer stateContainer { get; set; } = null!;

        public NoteContentDto? noteContent;
        public SharedNoteContentDto? sharedNoteContent;

        private NoteContentForm? noteContentForm;

        private int id;
        private bool hideUpdateBtn = false;
        private bool hideShareBtn = false;
        private bool hideDeleteBtn = false;
        private bool hideUnsubscribeBtn = true;


        protected async override Task OnInitializedAsync()
        {
            stateContainer.OnStateChange += MessageHandler;
            await RenderFormContent();
            
        }

        private async Task RenderFormContent()
        {
            if (stateContainer.noteId.Id != 0)
            {
                id = stateContainer.noteId.Id;
                if (!stateContainer.noteId.IsShare)
                {
                    noteContent = await notesDataService.GetNote(id);
                    if (noteContent != null)
                    {
                        noteContentForm = new NoteContentForm
                        {
                            Title = noteContent.Title,
                            Content = noteContent.Content
                        };
                        hideUpdateBtn = false;
                        hideShareBtn = false;
                        hideDeleteBtn = false;
                        hideUnsubscribeBtn = true;
                    }
                }
                else
                {
                    sharedNoteContent = await notesDataService.GetSharedNote(id);
                    if (sharedNoteContent != null)
                    {
                        noteContentForm = new NoteContentForm
                        {
                            Title = sharedNoteContent.Title,
                            Content = sharedNoteContent.Content
                        };
                        if (sharedNoteContent.PermissionId == 2)
                        {
                            hideUpdateBtn = false;
                        }
                        else
                        {
                            hideUpdateBtn = true;
                        }
                        hideShareBtn = true;
                        hideDeleteBtn = true;
                        hideUnsubscribeBtn = false;
                    }
                }
            }
        }

        private async Task OnBoardShareItem()
        {
            await stateContainer.GetSharedNoteUsers();
        }

        private async Task Submit()
        {
            if (noteContentForm != null)
            {
                var content = new NoteContentDto
                {
                    Title = noteContentForm.Title,
                    Content = noteContentForm.Content
                };
                await notesDataService.UpdateNote(id, content);
            }
           
        }

        private async Task Reset()
        {
            await notesDataService.DeleteNote(id);
            await stateContainer.GetNoteMetaData();
            navigation.NavigateTo("/");
        }

        private async void MessageHandler()
        {
            id = stateContainer.noteId.Id;
            await RenderFormContent();
            StateHasChanged();
        }

        private async Task Unsubscribe()
        {
            await notesDataService.UnSubscribeToNote(id);
            await stateContainer.GetSharedNoteMetaData();
            navigation.NavigateTo("/");
        }

        public void Dispose()
        {
            stateContainer.OnStateChange -= MessageHandler;
        }

        public class NoteContentForm
        {
            [Required]
            [MaxLength(100)]
             public string Title { get; set; } = string.Empty;
            [MaxLength(10000)]
             public string? Content { get; set; }
        }
    }


}

