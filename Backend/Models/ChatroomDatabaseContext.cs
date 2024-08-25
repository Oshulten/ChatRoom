using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class ChatroomDatabaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User.DbUser> Users { get; set; }
    public DbSet<Message.DbMessage> Messages { get; set; }
    public DbSet<Space.DbSpace> Spaces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Cd>()
        //     .Property<int>("Id");
        // modelBuilder.Entity<Cd>()
        //     .HasKey("Id");

        // modelBuilder.Entity<Genre>()
        //     .Property<int>("Id");
        // modelBuilder.Entity<Genre>()
        //     .HasKey("Id");
    }
}