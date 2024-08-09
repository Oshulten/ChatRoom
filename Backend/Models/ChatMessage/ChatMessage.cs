using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ChatMessage(Guid userId, string content, Guid chatSpace)
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; } = userId;
        public DateTime PostedAt { get; set; } = DateTime.Now;
        public string Content { get; set; } = content;
        public Guid ChatSpace { get; set; } = chatSpace;
        public ChatMessage() : this(Guid.NewGuid(), "A message", Guid.NewGuid()) { }
    }
}