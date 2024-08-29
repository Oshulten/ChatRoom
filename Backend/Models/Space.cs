using Backend.Database;
using Backend.Dto;

namespace Backend.Models;

public class Space(string alias)
{
    public int Id { get; set; }
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = alias;
    public List<User> Members { get; set; } = [];
    public List<Message> Messages { get; set; } = [];

    public static readonly Space Null = new("Null");
    public Space() : this(Null.Alias) { }
}