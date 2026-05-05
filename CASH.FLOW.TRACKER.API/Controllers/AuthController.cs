using CASH.FLOW.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Model.Response;
using CASH.FLOW.TRACKER.API.Services;
using CASH.FLOW.TRACKER.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens.Experimental;
using Sprache;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CASH.FLOW.TRACKER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            JwtTokenService jwtService,
            IEmailService emailService,
            IConfiguration config)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _emailService = emailService;
            _config = config;
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult<ReturnResponse<MeDto>> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);

            var userDetails = new MeDto(userId, email, userName);

            return Ok(new ReturnResponse<MeDto>
            {
                StatusCode = 200,
                Message = "Retrieve user details",
                Data = userDetails
            });
        }


        [HttpPost("register")]
        public async Task<ActionResult<ReturnResponse<string>>> Register([FromBody]RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.Email,
                Email = dto.Email,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "Registration Failed. Please try again",
                    Data = string.Empty
                });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            //var configSection = _config.GetSection("FRONTEND");
            var frontendUrl = _config["FRONTEND:DEVURL"];
            var confirmUrl = $"{frontendUrl}/confirm-email" +
                           $"?userId={user.Id}&token={encodedToken}";
            var body = $"<p>Hi {user.FullName}, <br>" +
                       "<p>Thank you for signing up! Please confirm your email address to activate your account.</p> <br>" +
                       $"<p>Click the link to verify your email: </p>" +
                       $"<a href='{confirmUrl}'>{confirmUrl}</a> <br>" +
                       "<p>If you didn't request this, please ignore this email.</p><br>" +
                       "<p>Your confirmation link will expire in <strong>24 hours</strong>, so verify your email soon!</p><br><br>" +
                       "<p>Cash Flow Tracker</p>";

            await _emailService.SendAsync(user.Email!,
                "Confirm Your Email - Action Required",
                body);

            return Ok(new ReturnResponse<string> {
                StatusCode = 200,
                Message = "Registration successful. Please confirm your email.",
                Data = string.Empty
            });
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult<ReturnResponse<string>>> ConfirmEmail([FromQuery] ConfirmEmailDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.userId);
            if (user is null)
            {
                return NotFound(new ReturnResponse<string>
                {
                    StatusCode = 404,
                    Message = "",
                    Data = string.Empty
                });
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            return result.Succeeded
                ? Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "Email confirmed.",
                    Data = string.Empty
                })
                : BadRequest(new ReturnResponse<string>
                {
                    StatusCode = 400,
                    Message = "Invalid or expired token.",
                    Data = string.Empty
                });
        }

        [HttpPost("login")]
        public async Task<ActionResult<ReturnResponse<string>>> Login([FromBody]LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return Unauthorized(new ReturnResponse<string>
                {
                    StatusCode = 401,
                    Message = "Invalid credentials",
                    Data = string.Empty
                });
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 401,
                    Message = "Please confirm your email first",
                    Data = string.Empty
                });
            }

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return Unauthorized(new ReturnResponse<string>
                {
                    StatusCode = 401,
                    Message = "Invalid credentials",
                    Data = string.Empty
                });
            }

            var token = _jwtService.GenerateToken(user);

            // Write JWT into an httpOnly cookie
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true, // JS cannot read this cookie
                Secure = true,  // only sent over HTTPS
                SameSite = SameSiteMode.None, // CSRF protection
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            });

            return Ok(new ReturnResponse<string>
            {
                StatusCode = 200,
                Message = "Login successfully",
                Data = string.Empty
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public ActionResult<ReturnResponse<string>> Logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                Path = "/",
                Secure = true,
                SameSite = SameSiteMode.None,
            });
            return Ok(new ReturnResponse<string>
            {
                StatusCode = 200,
                Message = "Logged out.",
                Data = string.Empty
            });
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ReturnResponse<string>>> ForgotPassword([FromBody]ForgotPasswordDto dto)
        {
            const string safeResponse = "If that email is registered you will receive a reset link.";

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = safeResponse,
                    Data = string.Empty
                });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var frontendUrl = _config["FRONTEND:DEVURL"];
            var resetUrl = $"{frontendUrl}/reset-password" + //change link
                           $"?userId={user.Id}&token={encodedToken}";
            var subject = "Reset Your Password – Action Required";
            var body = $"Click the link to reset your password: <a href='{resetUrl}'>Reset password</a>";

            await _emailService.SendAsync(user.Email!, subject, body);

            return Ok(new ReturnResponse<string>
            {
                StatusCode = 200,
                Message = safeResponse,
                Data = string.Empty
            });
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ReturnResponse<string>>> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null)
            {
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 400,
                    Message = "Invalid request",
                    Data = string.Empty
                });
            }

            var decodedUrl = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            var result = await _userManager.ResetPasswordAsync(user, decodedUrl, dto.NewPassword);
            
            return result.Succeeded
                ? Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "Password reset succesful",
                    Data = string.Empty
                })
                : Ok(new ReturnResponse<string>
                {
                    StatusCode = 400,
                    Message = $"{result.Errors}",
                    Data = string.Empty
                });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<ReturnResponse<string>>> ChangePassword([FromBody]ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

            if (user is null)
            {
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 401,
                    Message = "Invalid request",
                    Data = string.Empty
                });
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            return result.Succeeded
                ? Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "Password change succesful",
                    Data = string.Empty
                })
                : Ok(new ReturnResponse<string>
                {
                    StatusCode = 400,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Data = string.Empty
                });
        }

        [Authorize]
        [HttpPatch("update-profile")]
        public async Task<ActionResult<string>> UpdateProfile([FromBody] UpdateProfileDto dto) 
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if(user is null)
            {
                return Ok(new ReturnResponse<string>
                {
                    StatusCode = 401,
                    Message = "User not found",
                    Data = string.Empty
                });
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
                ? Ok(new ReturnResponse<string>
                {
                    StatusCode = 200,
                    Message = "Profile change succesfully",
                    Data = string.Empty
                })
                : Ok(new ReturnResponse<string>
                {
                    StatusCode = 400,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Data = string.Empty
                });
        }
    }
}

