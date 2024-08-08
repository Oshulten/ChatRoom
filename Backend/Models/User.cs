using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Member(string alias, string password)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Alias { get; set; } = alias;
        public string Password { get; set; } = password;

        // An explicit parameterless constructor is required for database creation
        public Member() : this("John Doe", "1337") { }

        public void Patch(Member patchObject)
        {
            Alias = patchObject.Alias;
            Password = patchObject.Password;
        }

        public static List<Member> SeedData()
        {
            return [
                new Member("Arnold", "MyNicePassword"),
                new Member("Catherine", "TheGreat"),
                new Member("TopGun91", "Join---us"),
            ];
        }
    }
}