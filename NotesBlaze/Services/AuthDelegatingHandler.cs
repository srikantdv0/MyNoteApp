using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace NotesBlaze.Services
{
    public class AuthDelegatingHandler : DelegatingHandler
    {

        private readonly IJSRuntime _jSRuntime;
        public AuthDelegatingHandler(IJSRuntime jSRuntime) //: base(new HttpClientHandler())
        {
            _jSRuntime = jSRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await GetJWT();
            if (!String.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string?> GetJWT()
        {
            var userdata = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", "user").ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(userdata))
            {
                var dataArray = userdata.Split(';', 2);
                if (dataArray.Length == 2)
                {
                    return dataArray[1];
                }
            }
            return null;
        }
    }
}

