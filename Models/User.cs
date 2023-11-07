using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace HeadHunter.Models
{
    public class User : IdentityUser<int>
    {
        public string? Avatar { get; set; }
    }
}
