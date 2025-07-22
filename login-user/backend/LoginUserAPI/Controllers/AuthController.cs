using Microsoft.AspNetCore.Mvc;

namespace LoginUserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        // In-memory storage (resets when app restarts)
        private static List<User> _users = new List<User>();
        private static int _nextId = 1;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Registration attempt for email: {Email}", request?.Email ?? "null");

                if (request == null)
                {
                    _logger.LogWarning("Registration request is null");
                    return BadRequest(new { message = "Invalid request data" });
                }

                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    _logger.LogWarning("Registration failed: Missing email or password");
                    return BadRequest(new { message = "Email and password are required" });
                }

                if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName))
                {
                    _logger.LogWarning("Registration failed: Missing first name or last name");
                    return BadRequest(new { message = "First name and last name are required" });
                }

                // Check if user already exists
                if (_users.Any(u => u.Email.ToLower() == request.Email.ToLower()))
                {
                    _logger.LogWarning("Registration failed: User already exists with email {Email}", request.Email);
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
                _logger.LogInformation("User registered successfully with ID: {UserId}", user.Id);

                var response = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                return Ok(new { message = "User registered successfully", user = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new { message = "Internal server error during registration" });
            }
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", request?.Email ?? "null");

                if (request == null)
                {
                    _logger.LogWarning("Login request is null");
                    return BadRequest(new { message = "Invalid request data" });
                }

                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    _logger.LogWarning("Login failed: Missing email or password");
                    return BadRequest(new { message = "Email and password are required" });
                }

                var user = _users.FirstOrDefault(u => 
                    u.Email.ToLower() == request.Email.ToLower() && 
                    u.Password == request.Password);

                if (user == null)
                {
                    _logger.LogWarning("Login failed: Invalid credentials for {Email}", request.Email);
                    return BadRequest(new { message = "Invalid email or password" });
                }

                _logger.LogInformation("User logged in successfully: {UserId}", user.Id);

                var response = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                return Ok(new { message = "Login successful", user = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                return StatusCode(500, new { message = "Internal server error during login" });
            }
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
            try
            {
                _logger.LogInformation("Profile update attempt for user ID: {UserId}", id);

                if (request == null)
                {
                    _logger.LogWarning("Update profile request is null");
                    return BadRequest(new { message = "Invalid request data" });
                }

                var user = _users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    _logger.LogWarning("Profile update failed: User not found with ID {UserId}", id);
                    return NotFound(new { message = "User not found" });
                }

                if (!string.IsNullOrEmpty(request.FirstName))
                    user.FirstName = request.FirstName;
                
                if (!string.IsNullOrEmpty(request.LastName))
                    user.LastName = request.LastName;

                _logger.LogInformation("Profile updated successfully for user ID: {UserId}", id);

                var response = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                return Ok(new { message = "Profile updated successfully", user = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile");
                return StatusCode(500, new { message = "Internal server error during profile update" });
            }
        }

        // Add a health check endpoint
        [HttpGet("health")]
        public ActionResult HealthCheck()
        {
            return Ok(new { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                userCount = _users.Count,
                message = "AuthController is running properly"
            });
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
