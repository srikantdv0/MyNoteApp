using System;
using System.ComponentModel.DataAnnotations;
using NotesShared.Models;

namespace NotesBlaze.Models
{
    
    public class LoginModel
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string email { get; set; } = String.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; } = String.Empty;
    }
    public class RegModel : LoginModel
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Password and confirm password do not match.")]
        public string confirmpwd { get; set; } = String.Empty;
    }

    public class ResetPassword
    {

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; } = String.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Password and confirm password do not match.")]
        public string confirmpwd { get; set; } = String.Empty;
    }

    public class NoteId
    {
        public int Id { get; set; }
        public bool IsShare { get; set; } = false;
        public string Title { get; set; } = String.Empty;
        public DateTime CreatedDTS { get; set; }
        public DateTime? ModifiedDTS { get; set; }
        public string? ModifiedByName { get; set; }
    }
}

