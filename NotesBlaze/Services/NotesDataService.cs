using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using NotesShared.Models;
using NotesBlaze.Models;
using NotesBlaze.Pages;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using NotesBlaze.Components;
using ResetPassword = NotesBlaze.Models.ResetPassword;
using System.Net.Mail;


namespace NotesBlaze.Services
{
    public class NotesDataService : INotesDataService
    {
        
        private readonly IApiCallHandler _apiCallHandler;
        private readonly NavigationManager _navigationManager;

        public NotesDataService(IApiCallHandler apiCallHandler, NavigationManager navigationManager)
        {
            _apiCallHandler = apiCallHandler;
            _navigationManager = navigationManager;
        }

        public async Task<UserProfileDto?> UserProfileAsync()
        {
            var response = await _apiCallHandler.ApiCall("Get", "api/User/userProfile", null);
            if (response != null)
            {
                return await response.Content.ReadFromJsonAsync<UserProfileDto>();
            }
            return null;
        }

        public async Task<string?> LoginAsync(LoginModel login)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(login);
          
            var response  = await _apiCallHandler.ApiCall("Post", "api/Authentication/authenticate", json);

            if (response != null)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        public async Task LogoutAsync()
        {
            await _apiCallHandler.ApiCall("Post", "api/Authentication/logout", null);
        }

        public async Task<string> RegisterAsync(UserForCreationDto userForCreationDto)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(userForCreationDto);
            var response = await _apiCallHandler.ApiCall("Post", "api/User/CreateUserAccount", json);
            if (response!=null)
            {
                return "Success";
            }
            return "Failed";
        }

        public async Task<IEnumerable<NoteMetadata>?> GetNotes()
        {

            var response = await _apiCallHandler.ApiCall("Get", "api/Notes/GetNotes", null);

            if (response != null && response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<NoteMetadata>>();
            }
            return null;
        }

        public async Task<NoteMetadata?> GetNotes(int Id)
        {
            var response = await _apiCallHandler.ApiCall("Get", $"api/Notes/GetNotes?Id={Id}", null);

            if (response != null)
            {
                return await response.Content.ReadFromJsonAsync<NoteMetadata>();
            }
            return null;
        }

        public async Task<IEnumerable<SharedNoteMetadata>?> GetSharedNotes()
        {
            var response = await _apiCallHandler.ApiCall("Get", "api/Notes/Getsharednotes", null);

            if (response != null)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<SharedNoteMetadata>>();
            }
            return null;
        }

        public async Task<SharedNoteMetadata?> GetSharedNotes(int Id)
        {
            var response = await _apiCallHandler.ApiCall("Get", $"api/Notes/Getsharednotes?Id={Id}", null);

            if (response != null)
            {
                return await response.Content.ReadFromJsonAsync<SharedNoteMetadata>();
            }
            return null;
            
        }

        public async Task<NoteContentDto?> GetNote(int id)
        {
            var response = await _apiCallHandler.ApiCall("Get", $"api/Notes/GetNote/{id}", null);

            if (response != null)
            {
                return await response.Content.ReadFromJsonAsync<NoteContentDto>();
            }
            return null;
        }

        public async Task<SharedNoteContentDto?> GetSharedNote(int id)
        {
            var response = await _apiCallHandler.ApiCall("Get", $"api/Notes/GetSharednote/{id}", null);

            if (response != null)
            {
                return await response.Content.ReadFromJsonAsync<SharedNoteContentDto>();
            }
            return null;
        }

        public async Task UpdateNote(int id, NoteContentDto noteforUpdate)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(noteforUpdate);

            await _apiCallHandler.ApiCall("Post", $"api/Notes/Updatenote/{id}", json);
        }

        public async Task<int?> CreateNote(NotesForCreationDto noteForCreation)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(noteForCreation);

            var response = await _apiCallHandler.ApiCall("Post", "api/Notes/Createnote", json);

            if (response != null)
            {
                return Int32.Parse(await response.Content.ReadAsStringAsync());
            }
            return null;
        }

        public async Task DeleteNote(int id)
        {
           await _apiCallHandler.ApiCall("Delete", $"api/Notes/DeleteNote/{id}", null);
        }

        public async Task<string> SendOTPAsync(ConfirmOTP confirmOTP)
        {
            var mail = new SendMailDto { ReciverEmail = confirmOTP.email, ReciverName = confirmOTP.email, Subject = "One time password", Body = $"Here's the OTP to reset your password {System.Environment.NewLine}" };
            var json = System.Text.Json.JsonSerializer.Serialize(mail);
            
            var response = await _apiCallHandler.ApiCall("Post", $"api/User/SendOtp", json);

            if (response != null && response.IsSuccessStatusCode)
            {
                return "OTP sent to your email address";
            }
            else
            {
                return "Failed to send OTP";
            }
        }

        public async Task<bool> VerifyOTPAsync(ConfirmOTP confirmOTP)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(confirmOTP);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _apiCallHandler.ApiCall("Post","api/User/VerifyOtp", json);
           
            if (response != null && response.IsSuccessStatusCode)
            {
                var isVerified = bool.Parse(await response.Content.ReadAsStringAsync());
                return isVerified;
            }
            return false;
        }

        public async Task<string> ForgetPassword(ConfirmOTP confirmOTP, ResetPassword resetPassword)
        {
            var isVerified = await VerifyOTPAsync(confirmOTP);
            if (!isVerified)
            {
                return "Incorrect OTP";
            }
            var json = System.Text.Json.JsonSerializer.Serialize(resetPassword.password);

            var response = await _apiCallHandler.ApiCall("Post", $"api/user/ForgetPassword/{confirmOTP.email}", json);

            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    NaviageToLogin();
                    return "Password reset success";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return "User not found!";
                }
            }
            return "An error occured while resetting password";
        }

        public async Task<IEnumerable<UserProfileDto>?> GetUsersAsync()
        {
            var response = await _apiCallHandler.ApiCall("Get", "api/user/userProfiles", null);

            if (response != null)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<UserProfileDto>>();
            }
            return null;
            
        }

        public async Task ShareNote(int userId, int noteId, string permission)
        {
            var shareNoteDto = new ShareToUserDTO
            {
                UserId = userId,
                NoteId = noteId,
                PermissionId = permission == "ReadWrite" ? 2 : 1

            };
            var json = System.Text.Json.JsonSerializer.Serialize(shareNoteDto);

            var response = await _apiCallHandler.ApiCall("Post", "api/Notes/Sharenote", json);
        }

        public async Task UnshareNote(int userId, int noteId)
        {
            await _apiCallHandler.ApiCall("Delete", $"api/Notes/UnSharenote/{noteId}?userId={userId}", null);
        }

        public async Task UnSubscribeToNote(int Id)
        {
            await _apiCallHandler.ApiCall("Delete", $"api/Notes/UnSuscribetonote/{Id}", null);
        }

        public async Task<IEnumerable<SharedNoteUsersDto>?> GetSharedNoteUsers(int NoteId)
        {
            var response = await _apiCallHandler.ApiCall("Get", $"api/Notes/GetSharedNoteUsers/{NoteId}", null);

            if (response != null)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<SharedNoteUsersDto>>();
            }
            return null;
        }

        public async Task<string> UploadProfilePic(ImageFile imageFile)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(imageFile);
            var response = await _apiCallHandler.ApiCall("Post", "api/User/UploadImage", json);
            
                if (response != null && response.IsSuccessStatusCode)
                {
                    return "Files uploaded";
                }
                return "Error uploading the file";
        }

        public async Task<ImageFile?> GetProfilePic()
        {
            var response = await _apiCallHandler.ApiCall("Get", "api/User/GetProfilePic", null);

            if (response != null)
            {
                return await response.Content.ReadFromJsonAsync<ImageFile>();
            }
            return null;
        }

        private void NaviageToLogin()
        {
            _navigationManager.NavigateTo("/login");
        }
    }
}

