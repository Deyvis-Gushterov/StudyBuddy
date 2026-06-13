using StudyBuddy.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Json;
using static StudyBuddy.Mobile.Models.AuthModels;

namespace StudyBuddy.Mobile.Services
{
    internal class AuthService
    {
        private readonly HttpClient http;
        private const string BaseUrl = "http://10.0.2.2:5291";

        public AuthService()
        {
            this.http = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            try
            {
                var response = await this.http.PostAsJsonAsync("/api/auth/login", new LoginRequest
                {
                    Email = email,
                    Password = password
                });

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();
                    return new AuthResult { Success = true, User = user };
                }

                return new AuthResult { Success = false, Message = "Invalid email or password." };
            }
            catch
            {
                return new AuthResult { Success = false, Message = "Could not reach the server." };
            }
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await this.http.PostAsJsonAsync("/api/auth/register", request);

                if (response.IsSuccessStatusCode)
                    return new AuthResult { Success = true, Message = "Account created!" };

                var error = await response.Content.ReadAsStringAsync();
                return new AuthResult { Success = false, Message = error };
            }
            catch
            {
                return new AuthResult { Success = false, Message = "Could not reach the server." };
            }
        }

    }
}
