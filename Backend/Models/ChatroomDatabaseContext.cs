using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class ChatroomDatabaseContext : DbContext
    {
        public DbSet<Member> Users { get; set; }
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
}