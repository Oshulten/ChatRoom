using Backend.Database;
using Backend.Dto;

namespace Backend.Models;

public class Space(string alias, List<User> members)
{
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = alias;
    public List<User> Members { get; set; } = members;

    public static readonly Space Null = new("Null", []);

    public Space() : this(Null.Alias, Null.Members) { }

    public static Space SpaceFromDtoSpace(DtoSpace dto, ChatroomDatabaseContext context) =>
        new(dto.Alias, [.. context.Users.Where(user => dto.MemberGuids.Contains(user.Guid))]);

    public static explicit operator Space(DtoSpacePost post) =>
        new(post.Alias, []);

    public static explicit operator DtoSpace(Space space) =>
        new(space.Alias, space.Guid, [.. space.Members.Select(member => member.Guid)]);

}