using System;
using System.Collections.Generic;
using System.Text;

namespace StudyBuddy.Mobile.Models
{
    internal class AuthModels
    {
        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class RegisterRequest
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Nationality { get; set; } = string.Empty;
            public int Age { get; set; }
        }

        public class AuthResult
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public User? User { get; set; }
        }
    }
}
