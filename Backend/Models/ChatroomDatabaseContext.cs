using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class ChatroomDatabaseContext : DbContext
{
    public DbSet<User.DbUser> ChatUsers { get; set; }
    public DbSet<Message.DbMessage> ChatMessages { get; set; }
    public DbSet<Space.DbSpace> ChatSpaces { get; set; }
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