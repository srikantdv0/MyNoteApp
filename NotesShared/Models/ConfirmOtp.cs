using System;
using System.ComponentModel.DataAnnotations;

namespace NotesShared.Models
{

        public class ConfirmOTP
        {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string email { get; set; } = String.Empty;

        [Required(ErrorMessage = "OTP is required")]
        [MinLength(6, ErrorMessage = "OTP should be 6 digits long")]
        [MaxLength(6, ErrorMessage = "OTP should be 6 digits long")]
        public string OTP { get; set; } = String.Empty;
    }
}
