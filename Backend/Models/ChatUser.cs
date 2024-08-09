using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class ChatUser(string alias, string password, DateTime joinedAt)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        [Required]
        [Length(3, 25, ErrorMessage = "Alias needs to be between 3 and 25 characters long")]
        public string Alias { get; set; } = alias;
        [Required]
        [Length(8, 25, ErrorMessage = "Password needs to be between 8 and 25 characters long")]
        public string Password { get; set; } = password;
        [Required(ErrorMessage = "JoinedAt date is required")]
        public DateTime JoinedAt { get; set; } = joinedAt;

        // An explicit parameterless constructor is required for database creation
        public ChatUser() : this("John Doe", "Password", DateTime.Now) { }

        public void Patch(ChatUser patchObject)
        {
            Alias = patchObject.Alias;
            Password = patchObject.Password;
        }

        public static List<ChatUser> SeedData()
        {
            return [
                new ChatUser("Arnold", "MyNicePassword", DateTime.Now),
                new ChatUser("Catherine", "TheGreat", DateTime.Now),
                new ChatUser("TopGun91", "Join---us", DateTime.Now),
            ];
        }
    }
}