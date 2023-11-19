//Not using, kept for future reference
//using System;
//using System.Net.Http.Json;
//using System.Security.Claims;
//using NotesBlaze.Components;
//using System.Text;
//using NotesBlaze.Models;
//using Microsoft.AspNetCore.Components.Authorization;
//using NotesShared.Models;

//namespace NotesBlaze.Services
//{
//    public class CustomAuthStateProvider : AuthenticationStateProvider
//    {
//        private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());


//        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//        {
//            return new AuthenticationState(claimsPrincipal);
//        }

//        public void SetAuthInfo(UserProfileDto userProfile)
//        {
//            var identity = new ClaimsIdentity(new[]{
//        new Claim(ClaimTypes.Email, userProfile.Email),
//        new Claim(ClaimTypes.Name, userProfile.Name),
//        new Claim("UserId", userProfile.UserId.ToString())
//    }, "AuthCookie");

//            claimsPrincipal = new ClaimsPrincipal(identity);
//            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//        }

//        public void ClearAuthInfo()
//        {
//            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
//            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//        }
//    }
//}

