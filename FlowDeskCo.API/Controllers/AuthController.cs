using FlowDeskCo.Application.Dtos;
using FlowDeskCo.Application.Interfaces;
using FlowDeskCo.Application.Services;
using FlowDeskCo.Infrastructure.Persistence;
using FlowDeskCo.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestateCo.Domain.Entities.CoreEntities;
using System;
using System.Numerics;
using System.Security.Claims;

namespace FlowDeskCo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenService _jwtService;
        private readonly ITenantProvider _tenantProvider;
        private readonly IEmailService _emailService;
        private readonly ITenantSettingsService _tenantSettingServices;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, JwtTokenService jwtService, ITenantProvider tenantProvider, IEmailService emailService, ITenantSettingsService tenantSettingsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _tenantProvider = tenantProvider;
            _emailService = emailService;
            _tenantSettingServices = tenantSettingsService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var clientId = _tenantProvider.GetTenantId();

            if (await _userManager.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict(new
                {
                    error = "EmailAlreadyExists",
                    message = "Email is already in use. Please choose another."
                });

            if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == dto.Phone))
                return Conflict(new
                {
                    error = "PhoneNumberAlreadyExists",
                    message = "Phone number is already use. Please choose another."
                });

            if (await _userManager.Users.AnyAsync(u => u.UserName == dto.Username))
                return Conflict(new
                {
                    error = "UsernameAlreadyExists",
                    message = "Username already exists. Please choose another."
                });

            var user = new User
            {
                FullName = dto.FullName,
                UserName = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.Phone,
                ClientId = clientId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "User");

            var code = new Random().Next(100000, 999999).ToString();
            await _userManager.SetAuthenticationTokenAsync(user, "Custom", "2FACode", code);
            await _emailService.SendEmailAsync(user.Email, "Your 2FA Code", $"Your 2FA code is :{code}");

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials.");

            var code = new Random().Next(100000, 999999).ToString();

            await _userManager.SetAuthenticationTokenAsync(user, "Custom", "2FACode", code);

            await _emailService.SendEmailAsync(user.Email, "Your 2FA Code", $"Your 2FA code is :{code}");

            return Ok(new { Message = "2FA code sent to your email." });


        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] Verify2FaDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid email.");

            var code = await _userManager.GetAuthenticationTokenAsync(user, "Custom", "2FACode");
            if (code != dto.Code)
                return Unauthorized("Invalid 2FA code.");

            await _userManager.RemoveAuthenticationTokenAsync(user, "Custom", "2FACode");

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName ?? ""),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim("ClientId", user.ClientId.ToString())
    };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = dto.RememberMe,
                ExpiresUtc = dto.RememberMe ? DateTimeOffset.UtcNow.AddDays(15) : DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
           
                    return Ok(new
                    {
                            Id = user.Id,
                            Username = user.UserName,
                            Name = user.FullName,
                            Email = dto.Email,
                            Phone = user.PhoneNumber,
                            Role = user.Role,
                            Environemt = user.ClientId
                    });   
        }

        [HttpGet("session")]
        [Authorize(AuthenticationSchemes = "MyCookieAuth")]
        public async Task<IActionResult> GetSession()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            return Ok(new
            {
                    Id = user.Id,
                    Username = user.UserName,
                    Name = user.FullName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Role = user.Role,
                    Environemt = user.ClientId
            });
        }

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = "MyCookieAuth")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if(user == null)
                return NotFound();

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var baseResetUrl = await _tenantSettingServices.GetSettingAsync("url") ?? "https://localhost:5173";

            var resetLink = $"{baseResetUrl}/reset-password?token={Uri.EscapeDataString(resetToken)}&email={Uri.EscapeDataString(dto.Email)}";

            string subject = "Reset your password";
            string body = $"Click the following link to reset your password: <a href='{resetLink}'>Reset Password</a>";


            await _emailService.SendEmailAsync(dto.Email, subject, body);

            return Ok(new { message = "If a user exists, a reset link has been sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound();

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors);
        }

    }
}
