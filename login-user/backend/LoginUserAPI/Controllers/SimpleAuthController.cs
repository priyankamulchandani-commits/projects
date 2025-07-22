using Microsoft.AspNetCore.Mvc;

namespace LoginUserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimpleAuthController : ControllerBase
    {
        // In-memory storage (resets when app restarts)
        private static List<SimpleUser> _users = new List<SimpleUser>();
        private static int _nextId = 1;

        [HttpPost("register")]
        public ActionResult Register([FromBody] SimpleRegisterRequest request)
        {
            // Check if user exists
            if (_users.Any(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "User already exists" });
            }

            // Create new user (plain text password)
            var user = new SimpleUser
            {
                Id = _nextId++,
                Email = request.Email,
                Password = request.Password, // Plain text - NOT secure
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.Now
            };

            _users.Add(user);

            return Ok(new { 
                message = "Registration successful", 
                user = new { 
                    user.Id, 
                    user.Email, 
                    user.FirstName, 
                    user.LastName 
                } 
            });
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] SimpleLoginRequest request)
        {
            // Find user by email and password (plain text comparison)
            var user = _users.FirstOrDefault(u => 
                u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid credentials" });
            }

            return Ok(new { 
                message = "Login successful", 
                user = new { 
                    user.Id, 
                    user.Email, 
                    user.FirstName, 
                    user.LastName 
                } 
            });
        }

        [HttpGet("users")]
        public ActionResult GetAllUsers()
        {
            return Ok(_users.Select(u => new { 
                u.Id, 
                u.Email, 
                u.FirstName, 
                u.LastName, 
                u.CreatedAt 
            }));
        }

        [HttpPut("profile/{id}")]
        public ActionResult UpdateProfile(int id, [FromBody] UpdateProfileRequest request)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            return Ok(new { 
                message = "Profile updated successfully", 
                user = new { 
                    user.Id, 
                    user.Email, 
                    user.FirstName, 
                    user.LastName 
                } 
            });
        }
    }

    // Simple models
    public class SimpleUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }

    public class SimpleLoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class SimpleRegisterRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }

    public class UpdateProfileRequest
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
