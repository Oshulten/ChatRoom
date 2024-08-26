using Backend.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class ChatroomDatabaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<DbUser> Users { get; set; }
    public DbSet<DbMessage> Messages { get; set; }
    public DbSet<DbSpace> Spaces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbUser>()
            .Property<int>("Id");
        modelBuilder.Entity<DbUser>()
            .HasKey("Id");

        modelBuilder.Entity<DbSpace>()
            .Property<int>("Id");
        modelBuilder.Entity<DbSpace>()
            .HasKey("Id");

        modelBuilder.Entity<DbMessage>()
            .Property<int>("Id");
        modelBuilder.Entity<DbMessage>()
            .HasKey("Id");
    }

    public DbSpace? SpaceByGuid(Guid spaceGuid) =>
        Spaces.FirstOrDefault(space => space.Guid == spaceGuid);

    public DbUser? UserByGuid(Guid userGuid) =>
        Users.FirstOrDefault(user => user.Guid == userGuid);

    public IEnumerable<DbSpace> SpacesByUserGuidMembership(Guid userGuid) =>
        Spaces.Where(space => space.Members.Select(member => member.Guid).Contains(userGuid));

    public void SeedDataWithBogus(int numberOfUsers, int numberOfSpaces, int numberOfMessages)
    {
        Messages.RemoveRange(Messages);
        Spaces.RemoveRange(Spaces);
        Users.RemoveRange(Users);

        var users = new Faker<DbUser>()
            .RuleFor(o => o.Alias, f => f.Person.UserName)
            .RuleFor(o => o.Password, f => f.Internet.Password(10, true))
            .RuleFor(o => o.JoinedAt, f => f.Date.Between(new DateTime(1991, 04, 26), DateTime.Now))
            .RuleFor(o => o.Admin, f => f.Random.Bool())
            .Generate(numberOfUsers);

        var adminUser = new DbUser()
        {
            Alias = "qwer",
            Password = "qwer",
            JoinedAt = new DateTime(1991, 04, 26),
            Admin = true
        };
        users.Add(adminUser);

        var spaces = new Faker<DbSpace>()
            .RuleFor(o => o.Alias, f => f.Internet.DomainWord())
            .RuleFor(o => o.Members, f => f.Random.ListItems(users))
            .Generate(numberOfSpaces);

        foreach (var space in spaces)
        {
            space.Members.Add(adminUser);
        }

        var messages = new Faker<DbMessage>()
            .RuleFor(o => o.Sender, f => f.Random.ListItem(users))
            .RuleFor(o => o.PostedAt, f => f.Date.Between(new DateTime(1991, 04, 26), DateTime.Now))
            .RuleFor(o => o.Content, f => f.Lorem.Paragraph())
            .RuleFor(o => o.Space, f => f.Random.ListItem(spaces))
            .Generate(numberOfMessages);

        Messages.AddRange(messages);
        Spaces.AddRange(spaces);
        Users.AddRange(users);

        SaveChanges();
    }


}