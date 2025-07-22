using Microsoft.AspNetCore.Mvc;

namespace LoginUserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // In-memory storage (resets when app restarts)
        private static List<User> _users = new List<User>();
        private static int _nextId = 1;

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            // Check if user already exists
            if (_users.Any(u => u.Email.ToLower() == request.Email.ToLower()))
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            var user = new User
            {
                Id = _nextId++,
                Email = request.Email,
                Password = request.Password, // Plain text for simplicity
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            _users.Add(user);

            var response = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return Ok(new { message = "User registered successfully", user = response });
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            var user = _users.FirstOrDefault(u => 
                u.Email.ToLower() == request.Email.ToLower() && 
                u.Password == request.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            var response = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return Ok(new { message = "Login successful", user = response });
        }

        [HttpGet("users")]
        public ActionResult GetAllUsers()
        {
            var users = _users.Select(u => new UserResponse
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).ToList();

            return Ok(users);
        }

        [HttpPut("profile/{id}")]
        public ActionResult UpdateProfile(int id, [FromBody] UpdateProfileRequest request)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            if (!string.IsNullOrEmpty(request.FirstName))
                user.FirstName = request.FirstName;
            
            if (!string.IsNullOrEmpty(request.LastName))
                user.LastName = request.LastName;

            var response = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return Ok(new { message = "Profile updated successfully", user = response });
        }
    }

    // Models
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class UpdateProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
