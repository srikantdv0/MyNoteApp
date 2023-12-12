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
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jSRuntime;
        private readonly NavigationManager _navigationManager;
        private readonly ToastService _toastService;

        public NotesDataService(HttpClient httpClient, IJSRuntime jSRuntime, NavigationManager navigationManager,
                               ToastService toastService)
        {
            _httpClient = httpClient;
            _jSRuntime = jSRuntime;
            _navigationManager = navigationManager;
            _toastService = toastService;
        }

        public async Task<(string Message, UserProfileDto? UserProfile)> UserProfileAsync()
        {

            var response1 = await _httpClient.GetAsync("/user/userProfile");
            if (response1.IsSuccessStatusCode)
            {
                var response = await response1.Content.ReadFromJsonAsync<UserProfileDto>();

                return ("Success", response);
            }
            _toastService.SetToast("Error fetching user details");
            return ("Error", null);
        }

        public async Task<string?> LoginAsync(LoginModel login)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(login);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync($"api/Authentication/authenticate", stringContent);
            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task<string> LogoutAsync()
        {
            var res = await _httpClient.PostAsync($"api/Authentication/logout", null);
            if (res.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                _toastService.SetToast("Failed to logout.");
                return "Failed";
            }
        }

        public async Task<string> RegisterAsync(UserForCreationDto userForCreationDto)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(userForCreationDto);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync("api/User/CreateUserAccount", stringContent);
            if (res.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                _toastService.SetToast("Failed to register. Please try again.");
                return await res.Content.ReadAsStringAsync();
            }

        }

        public async Task<IEnumerable<NoteMetadata>?> GetNotes()
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("api/Notes/GetNotes");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<NoteMetadata>>();
                }
            }
            _toastService.SetToast("Failed to fetch notes.");
            return null;
        }

        public async Task<NoteMetadata?> GetNotes(int Id)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"api/Notes/GetNotes?Id={Id}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    return await response.Content.ReadFromJsonAsync<NoteMetadata>()
                                ?? throw new NullReferenceException(nameof(response));
                }
            }
            return null;
        }

        public async Task<IEnumerable<SharedNoteMetadata>?> GetSharedNotes()
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("api/Notes/Getsharednotes");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<SharedNoteMetadata>>();
                }
            }
            _toastService.SetToast("Failed to fetch shared notes");
            return null;
        }

        public async Task<SharedNoteMetadata?> GetSharedNotes(int Id)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"api/Notes/Getsharednotes?Id={Id}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    return await response.Content.ReadFromJsonAsync<SharedNoteMetadata>()
                                    ?? throw new NullReferenceException(nameof(response));
                }
            }
            return null;
        }

        public async Task<NoteContentDto?> GetNote(int id)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/Notes/GetNote/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    return await response.Content.ReadFromJsonAsync<NoteContentDto>()
                                 ?? throw new NullReferenceException(nameof(response));
                }
            }
            return null;
        }

        public async Task<SharedNoteContentDto?> GetSharedNote(int id)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/Notes/GetSharednote/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    return await response.Content.ReadFromJsonAsync<SharedNoteContentDto>()
                                    ?? throw new NullReferenceException(nameof(response));
                }
            }
            return null;


        }

        public async Task<string?> UpdateNote(int id, NoteContentDto noteforUpdate)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            var json = System.Text.Json.JsonSerializer.Serialize(noteforUpdate);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/Notes/Updatenote/{id}", stringContent);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }

        public async Task<int?> CreateNote(NotesForCreationDto noteForCreation)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

            var json = System.Text.Json.JsonSerializer.Serialize(noteForCreation);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/Notes/Createnote", stringContent);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                return Int32.Parse(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
            
        }

        public async Task<string?> DeleteNote(int id)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"api/Notes/DeleteNote/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }

        }

        public async Task<string> SendOTPAsync(ConfirmOTP confirmOTP)
        {
            var mail = new SendMailDto { ReciverEmail = confirmOTP.email, ReciverName = confirmOTP.email, Subject = "One time password", Body = $"Here's the OTP to reset your password {System.Environment.NewLine}" };
            var json = System.Text.Json.JsonSerializer.Serialize(mail);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync("api/User/SendOtp", stringContent);
            if (res.IsSuccessStatusCode)
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
            var res = await _httpClient.PostAsync("api/User/VerifyOtp", stringContent);
            if (res.IsSuccessStatusCode)
            {

                var isVerified = bool.Parse(await res.Content.ReadAsStringAsync());
                //if (isVerified)
                //{
                //    _navigationManager.NavigateTo("/resetpassword");
                //}
                return isVerified;
            }
            return false;
        }

        public async Task<string> ForgetPassword(ConfirmOTP confirmOTP, ResetPassword resetPassword)
        {
            var isVerified = await VerifyOTPAsync(confirmOTP);
            if (!isVerified)
            {
                _navigationManager.NavigateTo("/forgetPassword");
                return "Incorrect OTP";
            }
            var json = System.Text.Json.JsonSerializer.Serialize(resetPassword.password);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync($"api/user/ForgetPassword/{confirmOTP.email}", stringContent);

            if (res.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("/login");
                return "Password reset success";
            }
            else if (System.Net.HttpStatusCode.NotFound.Equals(res.StatusCode))
            {
                return "User not found!";
            }
            return "An error occured while resetting password";
        }

        public async Task<IEnumerable<UserProfileDto>?> GetUsersAsync()
        {
            var token = await GetJWT();
                 if (String.IsNullOrEmpty(token))
                {
                  return null;
                 }

                _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("api/user/userProfiles");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                   await OnUnauthorized();
                return null;
                }
                else if (response.IsSuccessStatusCode)
                {
                if (response.Content != null)
                  {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<UserProfileDto>>()
                           ?? throw new NullReferenceException(nameof(response));
                  }
                }
                return null;
        }

        public async Task<string?> ShareNote(int userId, int noteId, string permission)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

            var shareNoteDto = new ShareToUserDTO
            {
                UserId = userId,
                NoteId = noteId,
                PermissionId = permission == "ReadWrite" ? 2 : 1

            };
            var json = System.Text.Json.JsonSerializer.Serialize(shareNoteDto);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

           var response =  await _httpClient.PostAsync($"api/Notes/Sharenote", stringContent);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }

        public async Task<string?> UnshareNote(int userId, int noteId)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

           var response =  await _httpClient.DeleteAsync($"api/Notes/UnSharenote/{noteId}?userId={userId}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }

        public async Task<string?> UnSubscribeToNote(int Id)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"api/Notes/UnSuscribetonote/{Id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }

        public async Task<IEnumerable<SharedNoteUsersDto>?> GetSharedNoteUsers(int NoteId)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"api/Notes/GetSharedNoteUsers/{NoteId}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<SharedNoteUsersDto>>()
                                        ?? throw new NullReferenceException(nameof(response));
                }
            }
            return null;
        }

        public async Task<string> UploadProfilePic(ImageFile imageFile)
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return "Failed to authenticate user";
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

            using (var msg = await _httpClient.PostAsJsonAsync<ImageFile>("/api/User/UploadImage", imageFile, System.Threading.CancellationToken.None))
            {
                if (msg.IsSuccessStatusCode)
                {
                    return $"Files uploaded";
                }
                return "Error uploading the file";
            }
        }

        public async Task<ImageFile?> GetProfilePic()
        {
            var token = await GetJWT();
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/User/GetProfilePic");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await OnUnauthorized();
                return null;
            }
            else if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    return await response.Content.ReadFromJsonAsync<ImageFile>();
                }
            }
            return null;
        }

        private async Task OnUnauthorized()
        {
            _toastService.SetToast("Session Expired, Please login again.");
            await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", "user").ConfigureAwait(false);
            NaviageToLogin();
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
            else
            {
                NaviageToLogin();
            }
            return null;
        }

        private void NaviageToLogin()
        {
            _navigationManager.NavigateTo("/login");
        }
    }
}

