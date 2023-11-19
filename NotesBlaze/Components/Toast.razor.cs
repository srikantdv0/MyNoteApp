using System;
using Microsoft.AspNetCore.Components;
using NotesBlaze.Services;

namespace NotesBlaze.Components
{
    public partial class Toast : ComponentBase
    {
        [Inject]
        ToastService toastService { get; set; } = default!;

        private string _toastMessage = String.Empty;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            toastService.ToasterChanged += OnToastMessageChangeEvent;
        }

        private void OnToastMessageChangeEvent(object? sender, string message)
        {
            _toastMessage = message;
            StateHasChanged();
        }

        private void OnClose()
        {
            _toastMessage = String.Empty;
            StateHasChanged();
        }
    }
}

