using System;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace NotesBlaze.Services
{
    public class ApiCallHandler : IApiCallHandler
    {

        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jSRuntime;
        private readonly NavigationManager _navigationManager;
        private readonly ToastService _toastService;
        

        public ApiCallHandler(HttpClient httpClient, IJSRuntime jSRuntime, NavigationManager navigationManager,
                               ToastService toastService)
        {
            _httpClient = httpClient;
            _jSRuntime = jSRuntime;
            _navigationManager = navigationManager;
            _toastService = toastService;
        }

        public async Task<HttpResponseMessage?> ApiCall(string requestType,string uri,string? jsonSerializedContent)
        {
            //check for offline/onlinc status comes here

            HttpResponseMessage responseMessage;
            if (requestType == "Post")
            {
                StringContent? stringContent = default!;
                if (jsonSerializedContent != null)
                {
                    stringContent = new StringContent(jsonSerializedContent, UnicodeEncoding.UTF8, "application/json");
                }
                
                responseMessage = await _httpClient.PostAsync(uri, stringContent);
            }
            else if (requestType == "Delete")
            {
                responseMessage = await _httpClient.DeleteAsync(uri);
            }
            else
            {
                responseMessage = await _httpClient.GetAsync(uri);
            }

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                ToastNotification("Server is offline, try after some time.");
                return null;
            }
            else if (!responseMessage.IsSuccessStatusCode)
            {
                ToastNotification("Failed to process the request");
                return null;
            }
            
            return responseMessage;
        }

        private async Task OnUnauthorized()
        {
            await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", "user").ConfigureAwait(false);
            NaviageToLogin();
        }

        private void ToastNotification(string message)
        {
            _toastService.SetToast(message);
        }

        private void NaviageToLogin()
        {
            _navigationManager.NavigateTo("/login");
        }
    }
}

