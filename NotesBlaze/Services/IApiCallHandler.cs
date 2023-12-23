using System;
namespace NotesBlaze.Services
{
    public interface IApiCallHandler
    {
        Task<HttpResponseMessage?> ApiCall(string requestType, string uri, string? jsonSerializedContent);
    }
}

