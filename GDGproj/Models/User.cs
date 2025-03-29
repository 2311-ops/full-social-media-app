

using System.ComponentModel.DataAnnotations.Schema;

namespace GDGproj.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string Role { get; set; } = "Regular";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        [NotMapped]
        public object? Password { get; internal set; }
    }
}
