using System;
using System.Collections.Generic;
using System.Text;

namespace StudyBuddy.Mobile.Models
{
    internal class User
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

    }
}
