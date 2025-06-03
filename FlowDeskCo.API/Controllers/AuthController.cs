using FlowDeskCo.Application.Dtos;
using FlowDeskCo.Application.Interfaces;
using FlowDeskCo.Infrastructure.Persistence;
using FlowDeskCo.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestateCo.Domain.Entities.CoreEntities;

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
        private readonly AppDbContext _appDbContext;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, JwtTokenService jwtService, ITenantProvider tenantProvider, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _tenantProvider = tenantProvider;
            _appDbContext = appDbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var clientId = _tenantProvider.GetTenantId();

            if (clientId == Guid.Empty)
                return BadRequest("Invalid client");

            var user = new User
            {
                FullName = dto.FullName,
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                ClientId = dto.ClientId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "User");

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var usera = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usera == null)
            {
                return NotFound("User not in DB");
            }
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials.");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Ok(new { Token = token });
        }
    }
}
