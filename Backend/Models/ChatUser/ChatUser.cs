namespace Backend.Models.ChatUser;

public class ChatUser(string alias, string password, bool admin, DateTime joinedAt)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = alias;
    public string Password { get; set; } = password;
    public DateTime JoinedAt { get; set; } = joinedAt;
    public bool Admin { get; set; } = admin;

    // An explicit parameterless constructor is required for database creation
    public ChatUser() : this("John Doe", "Password", false, DateTime.Now) { }

    public static explicit operator ChatUser(ChatUserPost post) => new(post.Alias, post.Password, post.Admin, post.JoinedAt);
    public static explicit operator ChatUserResponse(ChatUser user) => new(user.Id, user.Alias, user.JoinedAt, user.Admin);
    public static explicit operator ChatUser(LoginRequest request) => new(request.Username, request.Password, false, DateTime.Now);

    public void Patch(ChatUserPatch patchObject)
    {
        Alias = patchObject.Alias ?? Alias;
        Password = patchObject.Password ?? Password;
        Admin = patchObject.Admin ?? Admin;
    }
}