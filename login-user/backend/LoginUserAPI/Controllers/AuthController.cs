using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LoginUserAPI.Data;
using LoginUserAPI.DTOs;
using LoginUserAPI.Models;
using LoginUserAPI.Services;
using BCrypt.Net;

namespace LoginUserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        
        public AuthController(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "User with this email already exists" });
            }
            
            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            
            // Create new user
            var user = new User
            {
                Email = request.Email,
                Password = hashedPassword,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            // Generate JWT token
            var token = _jwtService.GenerateToken(user);
            
            var response = new AuthResponse
            {
                Token = token,
                User = new UserProfile
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedAt = user.CreatedAt
                }
            };
            
            return Ok(response);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            // Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
            
            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
            
            // Generate JWT token
            var token = _jwtService.GenerateToken(user);
            
            var response = new AuthResponse
            {
                Token = token,
                User = new UserProfile
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedAt = user.CreatedAt
                }
            };
            
            return Ok(response);
        }
        
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfile>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            
            var profile = new UserProfile
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt
            };
            
            return Ok(profile);
        }
        
        [HttpPut("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfile>> UpdateProfile(UpdateProfileRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            
            await _context.SaveChangesAsync();
            
            var profile = new UserProfile
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt
            };
            
            return Ok(profile);
        }
        
        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            // In a stateless JWT implementation, logout is handled on the client side
            // by removing the token from storage
            return Ok(new { message = "Logged out successfully" });
        }
    }
}