using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NotesBlaze.Models;
using NotesBlaze.Services;
using NotesShared.Models;

namespace NotesBlaze.Components
{
    public partial class Register : ComponentBase
    {
        [Inject]
        INotesDataService _notesDataService { get; set; } = default!;

        [Inject]
        NavigationManager _navigation { get; set; } = default!;

        [Inject]
        IJSRuntime _jSRuntime { get; set; } = default!;


        RegModel user = new RegModel();
        bool isDisabled = false;
        public string message = String.Empty;

        private async Task OnValid()
        {
            isDisabled = true;

            if (user.password != user.confirmpwd)
            {
                message = "Password and Confirm password do not match";
                isDisabled = false;
                return;
            }

            var userForCreationDTO = new UserForCreationDto()
            {
                Email = user.email,
                Name = user.Name,
                Password = user.password
            };
            var register = await _notesDataService.RegisterAsync(userForCreationDTO);

            if (register == "Success")
            {
                var login = new LoginModel { email = userForCreationDTO.Email, password = userForCreationDTO.Password };
                var token = await _notesDataService.LoginAsync(login);
                if (!String.IsNullOrEmpty(token))
                {
                    await _jSRuntime.InvokeVoidAsync("localStorage.setItem", "user", $"{userForCreationDTO.Email};{token}").ConfigureAwait(false);
                    _navigation.NavigateTo("/", true);
                }
                else
                {

                    message = "Invalid credientials";
                    isDisabled = false;
                }
            }
            else
            {
                message = register;
                isDisabled = false;
            }

        }
    }
}

