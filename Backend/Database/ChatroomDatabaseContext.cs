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

    public Space? SpaceByGuid(Guid guid) =>
        Spaces.FirstOrDefault(s => s.Guid == guid);

    public Message? MessageByGuid(Guid guid) =>
        Messages.FirstOrDefault(m => m.Guid == guid);

    public User? UserByGuid(Guid guid) =>
        Users.FirstOrDefault(u => u.Guid == guid);

    public User? UserByAuth(DtoAuthentication auth) =>
        Users.FirstOrDefault(user =>
            user.Alias == auth.Alias &&
            user.Password == auth.Password);

    public User? CreateUser(User user)
    {
        if (!Users.Contains(user))
        {
            Users.Add(user);
            SaveChanges();
            return user;
        }

        return null;
    }

    public bool CreateSpace(Space space)
    {
        if (!Spaces.Contains(space))
        {
            Spaces.Add(space);
            SaveChanges();
            return true;
        }

        return false;
    }

    public bool AddUserToSpace(Guid userGuid, Guid spaceGuid)
    {
        var user = UserByGuid(userGuid);
        var space = SpaceByGuid(spaceGuid);

        if (user == null || space == null)
            return false;

        space.Members.Add(user);
        user.Spaces.Add(space);
        SaveChanges();

        return true;
    }

    public bool AddUserToSpace(User user, Space space) =>
        AddUserToSpace(user.Guid, space.Guid);

    public Message? CreateMessage(Message message)
    {
        var user = UserByGuid(message.Sender.Guid);
        var space = SpaceByGuid(message.Space.Guid);

        if (user == null || space == null)
        {
            Console.WriteLine("Non-existant user or space!");
            return null;
        }


        Messages.Add(message);
        space.Messages.Add(message);
        user.Messages.Add(message);

        SaveChanges();

        return message;
    }

    public void Clear()
    {
        Users.RemoveRange(Users);
        Spaces.RemoveRange(Spaces);
        Messages.RemoveRange(Messages);

        SaveChanges();
    }

    public List<Space>? SpacesByUserGuid(Guid userGuid)
    {
        if (UserByGuid(userGuid) == null)
            return null;

        return Spaces
            .Where(space => space.Members.Select(m => m.Guid).Contains(userGuid))
            .ToList();
    }

    public List<Message>? MessagesBySpaceGuid(Guid spaceGuid, DateTime? before, int? numberOfMessages)
    {
        var space = SpaceByGuid(spaceGuid);

        if (space == null)
            return null;

        var messages = space.Messages.OrderByDescending(m => m.PostedAt).ToList();

        if (before != null)
            messages = messages.Where(m => m.PostedAt < before).ToList();

        if (numberOfMessages != null)
            messages = messages.Take(numberOfMessages ?? 0).ToList();

        return messages;
    }

    public void SeedData(int numberOfUsers, int numberOfSpaces, int numberOfMessages)
    {
        Clear();

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

        foreach (var user in users)
        {
            CreateUser(user);
        }

        var spaces = new Faker<Space>()
            .RuleFor(o => o.Alias, f => f.Internet.DomainWord())
            .Generate(numberOfSpaces);

        foreach (var space in spaces)
        {
            CreateSpace(space);
            foreach (var user in users)
            {
                AddUserToSpace(user, space);
            }
        }

        var messages = new Faker<Message>()
            .RuleFor(o => o.Sender, f => f.Random.ListItem(users))
            .RuleFor(o => o.PostedAt, f => f.Date.Between(new DateTime(1991, 04, 26), DateTime.Now))
            .RuleFor(o => o.Content, f => f.Lorem.Paragraph())
            .RuleFor(o => o.Space, f => f.Random.ListItem(spaces))
            .Generate(numberOfMessages);

        foreach (var message in messages)
        {
            CreateMessage(message);
        }

        SaveChanges();
    }
}