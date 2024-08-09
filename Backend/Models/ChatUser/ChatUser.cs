namespace Backend.Models;

public class ChatUser(string alias, string password, bool admin, DateTime joinedAt)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = alias;
    public string Password { get; set; } = password;
    public DateTime JoinedAt { get; set; } = joinedAt;
    public bool Admin { get; set; } = admin;

    // An explicit parameterless constructor is required for database creation
    public ChatUser() : this("John Doe", "Password", false, DateTime.Now) { }

    public void Patch(ChatUserPatch patchObject)
    {
        Alias = patchObject.Alias ?? Alias;
        Password = patchObject.Password ?? Password;
        Admin = patchObject.Admin ?? Admin;
    }

    public static List<ChatUser> SeedData()
    {
        return [
            new ChatUser("Arnold", "MyNicePassword", false, DateTime.Now),
                new ChatUser("Catherine", "TheGreat", false, DateTime.Now),
                new ChatUser("TopGun91", "Join---us", true, DateTime.Now),
            ];
    }
}