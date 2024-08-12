using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class ChatroomDatabaseContext : DbContext
{
    public DbSet<ChatUser.ChatUser> ChatUsers { get; set; }
    public DbSet<ChatMessage.ChatMessage> ChatMessages { get; set; }
    public DbSet<ChatSpace.ChatSpace> ChatSpaces { get; set; }
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