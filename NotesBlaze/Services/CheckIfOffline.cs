using System;
using Microsoft.JSInterop;

namespace NotesBlaze.Services
{
    public class CheckIfOffline
    {
        private readonly IJSRuntime _jSRuntime;
        public CheckIfOffline(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public bool IsOnline { get; set; }
        public bool IsInitialzed { get; set; } = false;

        [JSInvokable("Connection.StatusChanged")]
        public void OnConnectionStatusChanged(bool isOnline)
        {
            if (IsOnline != isOnline)
            {
                IsOnline = isOnline;
            }
        }

        public async Task InitializeAsync()
        {
            await _jSRuntime.InvokeVoidAsync("Connection.Initialize", DotNetObjectReference.Create(this));
            IsInitialzed = true;
        }

        public async ValueTask DisposeAsync()
        {
            await _jSRuntime.InvokeVoidAsync("Connection.Dispose");
        }
    }
}

