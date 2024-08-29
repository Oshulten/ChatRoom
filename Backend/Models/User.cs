using Backend.Dto;

namespace Backend.Models;

public class User(string alias, string password)
{
    public int Id { get; set; }
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = alias;
    public string Password { get; set; } = password;
    public DateTime JoinedAt { get; set; } = DateTime.Now;

    public List<Space> Spaces { get; set; } = [];
    public List<Message> Messages { get; set; } = [];
    public bool Admin { get; set; } = false;

    public static readonly User Null = new("Null", "Null");
    public User() : this(Null.Alias, Null.Password) { }
}