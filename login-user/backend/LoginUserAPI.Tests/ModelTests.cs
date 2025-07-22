using LoginUserAPI.Controllers;

namespace LoginUserAPI.Tests
{
    public class ModelTests
    {
        [Fact]
        public void User_DefaultValues_AreCorrect()
        {
            // Act
            var user = new User();

            // Assert
            Assert.Equal(0, user.Id);
            Assert.Equal(string.Empty, user.Email);
            Assert.Equal(string.Empty, user.Password);
            Assert.Equal(string.Empty, user.FirstName);
            Assert.Equal(string.Empty, user.LastName);
        }

        [Fact]
        public void User_SetProperties_WorksCorrectly()
        {
            // Arrange
            var user = new User();

            // Act
            user.Id = 1;
            user.Email = "test@example.com";
            user.Password = "password123";
            user.FirstName = "John";
            user.LastName = "Doe";

            // Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal("password123", user.Password);
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
        }

        [Fact]
        public void LoginRequest_DefaultValues_AreCorrect()
        {
            // Act
            var request = new LoginRequest();

            // Assert
            Assert.Equal(string.Empty, request.Email);
            Assert.Equal(string.Empty, request.Password);
        }

        [Fact]
        public void LoginRequest_SetProperties_WorksCorrectly()
        {
            // Arrange
            var request = new LoginRequest();

            // Act
            request.Email = "test@example.com";
            request.Password = "password123";

            // Assert
            Assert.Equal("test@example.com", request.Email);
            Assert.Equal("password123", request.Password);
        }

        [Fact]
        public void RegisterRequest_DefaultValues_AreCorrect()
        {
            // Act
            var request = new RegisterRequest();

            // Assert
            Assert.Equal(string.Empty, request.Email);
            Assert.Equal(string.Empty, request.Password);
            Assert.Equal(string.Empty, request.FirstName);
            Assert.Equal(string.Empty, request.LastName);
        }

        [Fact]
        public void RegisterRequest_SetProperties_WorksCorrectly()
        {
            // Arrange
            var request = new RegisterRequest();

            // Act
            request.Email = "test@example.com";
            request.Password = "password123";
            request.FirstName = "John";
            request.LastName = "Doe";

            // Assert
            Assert.Equal("test@example.com", request.Email);
            Assert.Equal("password123", request.Password);
            Assert.Equal("John", request.FirstName);
            Assert.Equal("Doe", request.LastName);
        }

        [Fact]
        public void UpdateProfileRequest_DefaultValues_AreCorrect()
        {
            // Act
            var request = new UpdateProfileRequest();

            // Assert
            Assert.Equal(string.Empty, request.FirstName);
            Assert.Equal(string.Empty, request.LastName);
        }

        [Fact]
        public void UpdateProfileRequest_SetProperties_WorksCorrectly()
        {
            // Arrange
            var request = new UpdateProfileRequest();

            // Act
            request.FirstName = "Jane";
            request.LastName = "Smith";

            // Assert
            Assert.Equal("Jane", request.FirstName);
            Assert.Equal("Smith", request.LastName);
        }

        [Fact]
        public void UserResponse_DefaultValues_AreCorrect()
        {
            // Act
            var response = new UserResponse();

            // Assert
            Assert.Equal(0, response.Id);
            Assert.Equal(string.Empty, response.Email);
            Assert.Equal(string.Empty, response.FirstName);
            Assert.Equal(string.Empty, response.LastName);
        }

        [Fact]
        public void UserResponse_SetProperties_WorksCorrectly()
        {
            // Arrange
            var response = new UserResponse();

            // Act
            response.Id = 1;
            response.Email = "test@example.com";
            response.FirstName = "John";
            response.LastName = "Doe";

            // Assert
            Assert.Equal(1, response.Id);
            Assert.Equal("test@example.com", response.Email);
            Assert.Equal("John", response.FirstName);
            Assert.Equal("Doe", response.LastName);
        }

        [Fact]
        public void UserResponse_FromUser_MapsCorrectly()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Password = "password123", // This should not be in response
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var response = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            // Assert
            Assert.Equal(user.Id, response.Id);
            Assert.Equal(user.Email, response.Email);
            Assert.Equal(user.FirstName, response.FirstName);
            Assert.Equal(user.LastName, response.LastName);
            // Password should not be exposed in response
        }

        [Theory]
        [InlineData("test@example.com", "password123", "John", "Doe")]
        [InlineData("user@domain.org", "secret", "Jane", "Smith")]
        [InlineData("admin@company.net", "admin123", "Admin", "User")]
        public void RegisterRequest_VariousInputs_WorksCorrectly(string email, string password, string firstName, string lastName)
        {
            // Act
            var request = new RegisterRequest
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };

            // Assert
            Assert.Equal(email, request.Email);
            Assert.Equal(password, request.Password);
            Assert.Equal(firstName, request.FirstName);
            Assert.Equal(lastName, request.LastName);
        }

        [Theory]
        [InlineData("test@example.com", "password123")]
        [InlineData("user@domain.org", "secret")]
        [InlineData("admin@company.net", "admin123")]
        public void LoginRequest_VariousInputs_WorksCorrectly(string email, string password)
        {
            // Act
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            // Assert
            Assert.Equal(email, request.Email);
            Assert.Equal(password, request.Password);
        }

        [Theory]
        [InlineData("John", "Doe")]
        [InlineData("Jane", "Smith")]
        [InlineData("Admin", "User")]
        public void UpdateProfileRequest_VariousInputs_WorksCorrectly(string firstName, string lastName)
        {
            // Act
            var request = new UpdateProfileRequest
            {
                FirstName = firstName,
                LastName = lastName
            };

            // Assert
            Assert.Equal(firstName, request.FirstName);
            Assert.Equal(lastName, request.LastName);
        }
    }
}
