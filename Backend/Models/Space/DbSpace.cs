using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.User;

namespace Backend.Models.Space;

public class DbSpace(string alias, List<DbUser> members)
{
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = alias;
    public List<DbUser> Members { get; set; } = members;

    public static readonly DbSpace Null = new("Null", []);

    public DbSpace() : this(Null.Alias, Null.Members) { }

    public static DbSpace SpaceFromDtoSpace(DtoSpace dto, ChatroomDatabaseContext context) =>
        new(dto.Alias, [.. context.Users.Where(user => dto.MemberGuids.Contains(user.Guid))]);
}