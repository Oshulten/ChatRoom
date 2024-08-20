namespace Backend.Models.User;

public class DbUser(string alias, string password, bool admin, DateTime joinedAt)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = alias;
    public string Password { get; set; } = password;
    public DateTime JoinedAt { get; set; } = joinedAt;
    public bool Admin { get; set; } = admin;

    // An explicit parameterless constructor is required for database creation
    public DbUser() : this("John Doe", "Password", false, DateTime.Now) { }

    public static explicit operator DtoUser(DbUser user) => new(user.Id, user.Alias, user.JoinedAt, user.Admin);

    public static explicit operator DbUser(DtoAuthentication request) => new(request.Username, request.Password, false, DateTime.Now);
}