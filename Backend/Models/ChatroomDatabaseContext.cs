using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class ChatroomDatabaseContext : DbContext
{
    public DbSet<User.DbUser> Users { get; set; }
    public DbSet<Message.DbMessage> Messages { get; set; }
    public DbSet<Space.DbSpace> Spaces { get; set; }
    public string DbPath { get; }
    private string _databaseFileName = "chatroom.db";

    public ChatroomDatabaseContext(DbContextOptions<ChatroomDatabaseContext> options)
    : base(options)
    {
        DbPath = System.IO.Path.Join(Environment.CurrentDirectory, "Data", _databaseFileName);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}