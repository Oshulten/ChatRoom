using Backend.Dto;

namespace Backend.Models;

public class User(string alias, string password, bool admin, DateTime joinedAt)
{
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = alias;
    public string Password { get; set; } = password;
    public DateTime JoinedAt { get; set; } = joinedAt;
    public bool Admin { get; set; } = admin;

    public static readonly User Null = new("Null", "Null", false, DateTime.Now);

    public User() : this(Null.Alias, Null.Password, Null.Admin, Null.JoinedAt) { }

    public static explicit operator DtoUser(User user) => new(user.Guid, user.Alias, user.JoinedAt, user.Admin);

    public static explicit operator User(DtoAuthentication request) => new(request.Alias, request.Password, false, DateTime.Now);
}