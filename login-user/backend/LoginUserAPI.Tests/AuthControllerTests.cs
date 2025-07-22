using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using LoginUserAPI.Controllers;
using System.Reflection;

namespace LoginUserAPI.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockLogger = new Mock<ILogger<AuthController>>();
            _controller = new AuthController(_mockLogger.Object);
            
            // Clear static users list before each test
            ClearUsersForTesting();
        }

        private void ClearUsersForTesting()
        {
            // Use reflection to clear the static users list
            var usersField = typeof(AuthController).GetField("_users", BindingFlags.NonPublic | BindingFlags.Static);
            var nextIdField = typeof(AuthController).GetField("_nextId", BindingFlags.NonPublic | BindingFlags.Static);
            
            if (usersField != null)
            {
                var usersList = usersField.GetValue(null) as System.Collections.IList;
                usersList?.Clear();
            }
            
            if (nextIdField != null)
            {
                nextIdField.SetValue(null, 1);
            }
        }

        [Fact]
        public void Register_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var result = _controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            
            var response = okResult.Value.GetType().GetProperty("message")?.GetValue(okResult.Value);
            Assert.Equal("User registered successfully", response);
        }

        [Fact]
        public void Register_NullRequest_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Register(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Register_EmptyEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var result = _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value);
            Assert.Equal("Email and password are required", response);
        }

        [Fact]
        public void Register_EmptyPassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "",
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var result = _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value);
            Assert.Equal("Email and password are required", response);
        }

        [Fact]
        public void Register_EmptyFirstName_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "",
                LastName = "Doe"
            };

            // Act
            var result = _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value);
            Assert.Equal("First name and last name are required", response);
        }

        [Fact]
        public void Register_EmptyLastName_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = ""
            };

            // Act
            var result = _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value);
            Assert.Equal("First name and last name are required", response);
        }

        [Fact]
        public void Register_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var request1 = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
            
            var request2 = new RegisterRequest
            {
                Email = "test@example.com", // Same email
                Password = "password456",
                FirstName = "Jane",
                LastName = "Smith"
            };

            // Act
            _controller.Register(request1); // First registration
            var result = _controller.Register(request2); // Duplicate registration

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value);
            Assert.Equal("User with this email already exists", response);
        }

        [Fact]
        public void Register_DuplicateEmailCaseInsensitive_ReturnsBadRequest()
        {
            // Arrange
            var request1 = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
            
            var request2 = new RegisterRequest
            {
                Email = "TEST@EXAMPLE.COM", // Same email, different case
                Password = "password456",
                FirstName = "Jane",
                LastName = "Smith"
            };

            // Act
            _controller.Register(request1);
            var result = _controller.Register(request2);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value);
            Assert.Equal("User with this email already exists", response);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
            _controller.Register(registerRequest);

            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var result = _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value.GetType().GetProperty("message")?.GetValue(okResult.Value);
            Assert.Equal("Login successful", response);
        }

        [Fact]
        public void Login_NullRequest_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Login(null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void Login_EmptyEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "",
                Password = "password123"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value);
            Assert.Equal("Email and password are required", response);
        }

        [Fact]
        public void Login_EmptyPassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = ""
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value);
            Assert.Equal("Email and password are required", response);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
            _controller.Register(registerRequest);

            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            // Act
            var result = _controller.Login(loginRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value);
            Assert.Equal("Invalid email or password", response);
        }

        [Fact]
        public void Login_NonExistentUser_ReturnsBadRequest()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "nonexistent@example.com",
                Password = "password123"
            };

            // Act
            var result = _controller.Login(loginRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value);
            Assert.Equal("Invalid email or password", response);
        }

        [Fact]
        public void Login_CaseInsensitiveEmail_ReturnsOkResult()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
            _controller.Register(registerRequest);

            var loginRequest = new LoginRequest
            {
                Email = "TEST@EXAMPLE.COM", // Different case
                Password = "password123"
            };

            // Act
            var result = _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value.GetType().GetProperty("message")?.GetValue(okResult.Value);
            Assert.Equal("Login successful", response);
        }

        [Fact]
        public void GetAllUsers_EmptyList_ReturnsEmptyArray()
        {
            // Act
            var result = _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<System.Collections.IEnumerable>(okResult.Value);
            Assert.Empty(users.Cast<object>());
        }

        [Fact]
        public void GetAllUsers_WithUsers_ReturnsUserList()
        {
            // Arrange
            var request1 = new RegisterRequest
            {
                Email = "user1@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
            var request2 = new RegisterRequest
            {
                Email = "user2@example.com",
                Password = "password456",
                FirstName = "Jane",
                LastName = "Smith"
            };

            _controller.Register(request1);
            _controller.Register(request2);

            // Act
            var result = _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<System.Collections.IEnumerable>(okResult.Value);
            Assert.Equal(2, users.Cast<object>().Count());
        }

        [Fact]
        public void UpdateProfile_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
            var registerResult = _controller.Register(registerRequest);
            var okRegisterResult = Assert.IsType<OkObjectResult>(registerResult);
            var user = okRegisterResult.Value.GetType().GetProperty("user")?.GetValue(okRegisterResult.Value);
            var userId = (int)user.GetType().GetProperty("Id").GetValue(user);

            var updateRequest = new UpdateProfileRequest
            {
                FirstName = "Jane",
                LastName = "Smith"
            };

            // Act
            var result = _controller.UpdateProfile(userId, updateRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value.GetType().GetProperty("message")?.GetValue(okResult.Value);
            Assert.Equal("Profile updated successfully", response);
        }

        [Fact]
        public void UpdateProfile_NullRequest_ReturnsBadRequest()
        {
            // Act
            var result = _controller.UpdateProfile(1, null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void UpdateProfile_NonExistentUser_ReturnsNotFound()
        {
            // Arrange
            var updateRequest = new UpdateProfileRequest
            {
                FirstName = "Jane",
                LastName = "Smith"
            };

            // Act
            var result = _controller.UpdateProfile(999, updateRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = notFoundResult.Value.GetType().GetProperty("message")?.GetValue(notFoundResult.Value);
            Assert.Equal("User not found", response);
        }

        [Fact]
        public void UpdateProfile_EmptyFirstName_UpdatesOnlyLastName()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
            var registerResult = _controller.Register(registerRequest);
            var okRegisterResult = Assert.IsType<OkObjectResult>(registerResult);
            var user = okRegisterResult.Value.GetType().GetProperty("user")?.GetValue(okRegisterResult.Value);
            var userId = (int)user.GetType().GetProperty("Id").GetValue(user);

            var updateRequest = new UpdateProfileRequest
            {
                FirstName = "", // Empty first name
                LastName = "Smith"
            };

            // Act
            var result = _controller.UpdateProfile(userId, updateRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedUser = okResult.Value.GetType().GetProperty("user")?.GetValue(okResult.Value);
            var firstName = updatedUser.GetType().GetProperty("FirstName").GetValue(updatedUser);
            var lastName = updatedUser.GetType().GetProperty("LastName").GetValue(updatedUser);
            
            Assert.Equal("John", firstName); // Should remain unchanged
            Assert.Equal("Smith", lastName); // Should be updated
        }

        [Fact]
        public void HealthCheck_ReturnsOkResult()
        {
            // Act
            var result = _controller.HealthCheck();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            
            var status = okResult.Value.GetType().GetProperty("status")?.GetValue(okResult.Value);
            Assert.Equal("healthy", status);
        }

        [Fact]
        public void HealthCheck_ReturnsCorrectUserCount()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
            _controller.Register(request);

            // Act
            var result = _controller.HealthCheck();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var userCount = okResult.Value.GetType().GetProperty("userCount")?.GetValue(okResult.Value);
            Assert.Equal(1, userCount);
        }

        [Fact]
        public void Register_MultipleUsers_AssignsIncrementalIds()
        {
            // Arrange
            var request1 = new RegisterRequest
            {
                Email = "user1@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
            var request2 = new RegisterRequest
            {
                Email = "user2@example.com",
                Password = "password456",
                FirstName = "Jane",
                LastName = "Smith"
            };

            // Act
            var result1 = _controller.Register(request1);
            var result2 = _controller.Register(request2);

            // Assert
            var okResult1 = Assert.IsType<OkObjectResult>(result1);
            var okResult2 = Assert.IsType<OkObjectResult>(result2);
            
            var user1 = okResult1.Value.GetType().GetProperty("user")?.GetValue(okResult1.Value);
            var user2 = okResult2.Value.GetType().GetProperty("user")?.GetValue(okResult2.Value);
            
            var id1 = (int)user1.GetType().GetProperty("Id").GetValue(user1);
            var id2 = (int)user2.GetType().GetProperty("Id").GetValue(user2);
            
            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
        }
    }
}
