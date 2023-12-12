using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Entities;
using NotesShared.Models;
using Notes.Services;
using System.Net.Http;

namespace Notes.Controllers
{
	[ApiController]
	[Route("api/[Controller]")]
	public class UserController : Controller
	{
		private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ISendEmail _sendEmail;
        private readonly IWebHostEnvironment _env;

        public UserController(IUserRepository userRepository, IHttpContextAccessor httpContext
			,IMapper mapper, ISendEmail sendEmail, IWebHostEnvironment env)
		{
			_userRepository = userRepository;
			_mapper = mapper;
			_sendEmail = sendEmail;
			_env = env;
        }

		[HttpPost("CreateUserAccount")]
		public async Task<ActionResult> CreateUserAccount(UserForCreationDto userForCreationDto)
		{
			var user = await _userRepository.GetUserAsync(userForCreationDto.Email);
			if (user != null)
			{
				return BadRequest("User already exist");
			}

			var newUser = _mapper.Map<User>(userForCreationDto);
			newUser.CreatedDTS = DateTime.UtcNow;
			newUser.IsActive = true;
			await _userRepository.CreateAccountAsync(newUser);
			await _userRepository.SaveChangesAsync();
			return NoContent();
		}

        [Authorize]
        [HttpPost("ResetPassword")]
		public async Task<ActionResult> ResetPassword([FromBody]string Password)
		{
			
			var email = User.Claims.Where(d => d.Type == "UserEmailAddress")
					.Select(d => d.Value).FirstOrDefault();
			if (email == null)
			{
				return Forbid("Can't valid user's identity"); ;
			}
            var user = await _userRepository.GetUserAsync(email);
			if (user == null)
			{
				return NotFound();
			}
			user.Password = Password;

            _userRepository.ResetPasswordAsync(user);
            await _userRepository.SaveChangesAsync();
            return NoContent();
		}

		[HttpPost("ForgetPassword/{emailAddress}")]
		public async Task<ActionResult> ForgetPassword(string emailAddress, [FromBody]string newPassword)
		{
			var user = await _userRepository.GetUserAsync(emailAddress);
            if (user == null)
            {
                return NotFound();
            }

            user.Password = newPassword;
            _userRepository.ResetPasswordAsync(user);
            await _userRepository.SaveChangesAsync();
            return NoContent();
        }

		[HttpPost("SendOtp")]
		public async Task<ActionResult> SendOpt(SendMailDto sendMailDto)
		{

            int otp = OtpGenerator(100000, 999999);

            if (! await _sendEmail.SendEmailAsync(sendMailDto.ReciverName, sendMailDto.ReciverEmail
				, sendMailDto.Subject, sendMailDto.Body + otp))
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error sending email");
			}
			var toSave = new Otp()
			{
				EmailId = sendMailDto.ReciverEmail,
				ValidTill = DateTime.UtcNow.AddMinutes(5),
				Code = otp
			};
			await _userRepository.AddOtp(toSave);
			await _userRepository.SaveChangesAsync();
			return Ok("Mail Sent");
		}

		[HttpPost("VerifyOtp")]
		public async Task<IActionResult> VerifyOtp(ConfirmOTP confirmOTP)
		{
			var res = await _userRepository.GetOtp(confirmOTP.email);
			if (res == 0)
			{
				return Ok(false);
			}
			else
			{
				int otp;
				if (int.TryParse(confirmOTP.OTP, out otp))
				{
					if (otp == res)
					{
                        return Ok(true);
                    }
				}
			}
			return Ok(false);
		}

		//To be removed
		[HttpGet("GetUserDetails")]
		public async Task<ActionResult<IEnumerable<Otp>>> GetUser()
		{
			return Ok(await _userRepository.GetOtps());
        }

		[Authorize]
		[HttpGet("userProfile")]
        public async Task<IActionResult> UserProfileAsync()
        {
            var email = User.Claims.Where(d => d.Type == "UserEmailAddress")
                    .Select(d => d.Value).FirstOrDefault();
			if (email != null)
			{
				var user = await _userRepository.GetUserAsync(email);
				if (user != null)
				{
					var userProfile = new UserProfileDto
					{
						UserId = user.Id,
						Email = user.Email,
						Name = user.Name
					};
					return Ok(userProfile);
				}
			}
				return NotFound();
        }

		[Authorize]
		[HttpGet("userProfiles")]
		public async Task<IActionResult> UserProfilesAsync()
		{
            var users = await _userRepository.GetUsersAsync();
            if (users != null)
            {
                var usersDTO = _mapper.Map<IEnumerable<UserProfileDto>>(users);
                return Ok(usersDTO);
            }
            else
            {
                return NotFound();
            }
        }

		[Authorize]
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromBody] ImageFile file)
        {
            var email = User.Claims.Where(d => d.Type == "UserEmailAddress")
                    .Select(d => d.Value).FirstOrDefault();
			if (email == null)
			{
                return Unauthorized();
            }
            var user = await _userRepository.GetUserAsync(email);
			if (user == null)
			{
                    return Unauthorized();
            }

			var buf = Convert.FromBase64String(file.Base64data);
			await System.IO.File.WriteAllBytesAsync(_env.ContentRootPath + "UserProfileImages" + System.IO.Path.DirectorySeparatorChar + user.Id, buf);
			return Ok();
			
        }

		[Authorize]
		[HttpGet("GetProfilePic")]
		public async Task<ActionResult<ImageFile>> GetProfilePic()
		{
            var email = User.Claims.Where(d => d.Type == "UserEmailAddress")
                    .Select(d => d.Value).FirstOrDefault();
            if (email == null)
            {
                return Unauthorized();
            }
            var user = await _userRepository.GetUserAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }
			var filePath = _env.ContentRootPath + "UserProfileImages" + System.IO.Path.DirectorySeparatorChar + user.Id;

            if (!System.IO.File.Exists(filePath))
			{
				return NotFound();
			}

			Byte[] bytes = await System.IO.File.ReadAllBytesAsync(filePath);
			string file = Convert.ToBase64String(bytes);
            
            var fileBase64 = new ImageFile { Base64data = file, ContentType = "Image/PNG", FileName = $"{user.Id}" }; // convert to a base64 string!!
			return Ok(fileBase64);
        }

        private int OtpGenerator(int min, int max)
		{
			var random = new Random();
			return random.Next(min,max);
		}
	}
}

