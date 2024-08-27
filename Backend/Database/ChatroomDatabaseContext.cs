using Backend.Dto;
using Backend.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class ChatroomDatabaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Space> Spaces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property<int>("Id");
        modelBuilder.Entity<User>()
            .HasKey("Id");

        modelBuilder.Entity<Space>()
            .Property<int>("Id");
        modelBuilder.Entity<Space>()
            .HasKey("Id");

        modelBuilder.Entity<Message>()
            .Property<int>("Id");
        modelBuilder.Entity<Message>()
            .HasKey("Id");
    }

    public Space? SpaceByGuid(Guid spaceGuid) =>
        Spaces.FirstOrDefault(space => space.Guid == spaceGuid);

    public User? UserByGuid(Guid userGuid) =>
        Users.FirstOrDefault(user => user.Guid == userGuid);

    public IEnumerable<Space> SpacesByUserGuidMembership(Guid userGuid) =>
        Spaces.Where(space => space.Members.Select(member => member.Guid).Contains(userGuid));

    //Casting
    public DtoUser DtoUserFromUser(User user) =>
        new(user.Guid, user.Alias, user.JoinedAt, user.Admin);

    public User UserFromDtoAuthentication(DtoAuthentication auth) =>
        new(auth.Alias, auth.Password, false, DateTime.Now);

    public async Task<User?> GetUserFromAuthentication(DtoAuthentication auth) =>
        await Users.FirstOrDefaultAsync(user =>
            user.Alias == auth.Alias &&
            user.Password == auth.Alias);

    public async Task<User?> CreateUser(DtoAuthentication auth)
    {
        var existingUser = Users.FirstOrDefault(user =>
            user.Alias == auth.Alias);

        if (existingUser is not null) return null;

        await Users.AddAsync(existingUser!);
        await SaveChangesAsync();

        return existingUser;
    }

    public void SeedData(int numberOfUsers, int numberOfSpaces, int numberOfMessages)
    {
        Messages.RemoveRange(Messages);
        Spaces.RemoveRange(Spaces);
        Users.RemoveRange(Users);

        var users = new Faker<User>()
            .RuleFor(o => o.Alias, f => f.Person.UserName)
            .RuleFor(o => o.Password, f => f.Internet.Password(10, true))
            .RuleFor(o => o.JoinedAt, f => f.Date.Between(new DateTime(1991, 04, 26), DateTime.Now))
            .RuleFor(o => o.Admin, f => f.Random.Bool())
            .Generate(numberOfUsers);

        var adminUser = new User()
        {
            Alias = "qwer",
            Password = "qwer",
            JoinedAt = new DateTime(1991, 04, 26),
            Admin = true
        };
        users.Add(adminUser);

        var spaces = new Faker<Space>()
            .RuleFor(o => o.Alias, f => f.Internet.DomainWord())
            .RuleFor(o => o.Members, f => f.Random.ListItems(users))
            .Generate(numberOfSpaces);

        foreach (var space in spaces)
        {
            space.Members.Add(adminUser);
        }

        var messages = new Faker<Message>()
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