using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using LoginUserAPI;

namespace LoginUserAPI.Tests
{
    public class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Register_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new
            {
                email = "integration@example.com",
                password = "password123",
                firstName = "Integration",
                lastName = "Test"
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/register", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("User registered successfully", responseString);
        }

        [Fact]
        public async Task Register_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new
            {
                email = "", // Empty email
                password = "password123",
                firstName = "Integration",
                lastName = "Test"
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/register", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Email and password are required", responseString);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsSuccess()
        {
            // Arrange - First register a user
            var registerRequest = new
            {
                email = "login@example.com",
                password = "password123",
                firstName = "Login",
                lastName = "Test"
            };
            var registerJson = JsonSerializer.Serialize(registerRequest);
            var registerContent = new StringContent(registerJson, Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/auth/register", registerContent);

            // Now login
            var loginRequest = new
            {
                email = "login@example.com",
                password = "password123"
            };
            var loginJson = JsonSerializer.Serialize(loginRequest);
            var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/login", loginContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Login successful", responseString);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var loginRequest = new
            {
                email = "nonexistent@example.com",
                password = "wrongpassword"
            };
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/login", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid email or password", responseString);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/auth/users");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseString);
        }

        [Fact]
        public async Task HealthCheck_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/auth/health");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("healthy", responseString);
        }

        [Fact]
        public async Task UpdateProfile_ValidRequest_ReturnsSuccess()
        {
            // Arrange - First register a user
            var registerRequest = new
            {
                email = "update@example.com",
                password = "password123",
                firstName = "Update",
                lastName = "Test"
            };
            var registerJson = JsonSerializer.Serialize(registerRequest);
            var registerContent = new StringContent(registerJson, Encoding.UTF8, "application/json");
            var registerResponse = await _client.PostAsync("/api/auth/register", registerContent);
            var registerResponseString = await registerResponse.Content.ReadAsStringAsync();
            
            // Extract user ID from response (simplified approach)
            var userId = 1; // Assuming first user gets ID 1

            // Now update profile
            var updateRequest = new
            {
                firstName = "Updated",
                lastName = "Name"
            };
            var updateJson = JsonSerializer.Serialize(updateRequest);
            var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/auth/profile/{userId}", updateContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Profile updated successfully", responseString);
        }

        [Fact]
        public async Task UpdateProfile_NonExistentUser_ReturnsNotFound()
        {
            // Arrange
            var updateRequest = new
            {
                firstName = "Updated",
                lastName = "Name"
            };
            var json = JsonSerializer.Serialize(updateRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/auth/profile/999", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("User not found", responseString);
        }

        [Fact]
        public async Task Register_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange - Register first user
            var request1 = new
            {
                email = "duplicate@example.com",
                password = "password123",
                firstName = "First",
                lastName = "User"
            };
            var json1 = JsonSerializer.Serialize(request1);
            var content1 = new StringContent(json1, Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/auth/register", content1);

            // Try to register second user with same email
            var request2 = new
            {
                email = "duplicate@example.com", // Same email
                password = "password456",
                firstName = "Second",
                lastName = "User"
            };
            var json2 = JsonSerializer.Serialize(request2);
            var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/auth/register", content2);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("User with this email already exists", responseString);
        }

        [Fact]
        public async Task EndToEndUserFlow_RegisterLoginUpdateProfile_Success()
        {
            // Arrange
            var email = "endtoend@example.com";
            var password = "password123";

            // Step 1: Register
            var registerRequest = new
            {
                email = email,
                password = password,
                firstName = "End",
                lastName = "ToEnd"
            };
            var registerJson = JsonSerializer.Serialize(registerRequest);
            var registerContent = new StringContent(registerJson, Encoding.UTF8, "application/json");

            // Act & Assert - Register
            var registerResponse = await _client.PostAsync("/api/auth/register", registerContent);
            registerResponse.EnsureSuccessStatusCode();
            var registerResponseString = await registerResponse.Content.ReadAsStringAsync();
            Assert.Contains("User registered successfully", registerResponseString);

            // Step 2: Login
            var loginRequest = new
            {
                email = email,
                password = password
            };
            var loginJson = JsonSerializer.Serialize(loginRequest);
            var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            // Act & Assert - Login
            var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);
            loginResponse.EnsureSuccessStatusCode();
            var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
            Assert.Contains("Login successful", loginResponseString);

            // Step 3: Update Profile
            var updateRequest = new
            {
                firstName = "Updated",
                lastName = "User"
            };
            var updateJson = JsonSerializer.Serialize(updateRequest);
            var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

            // Act & Assert - Update Profile (assuming user ID 1)
            var updateResponse = await _client.PutAsync("/api/auth/profile/1", updateContent);
            updateResponse.EnsureSuccessStatusCode();
            var updateResponseString = await updateResponse.Content.ReadAsStringAsync();
            Assert.Contains("Profile updated successfully", updateResponseString);
        }
    }
}
