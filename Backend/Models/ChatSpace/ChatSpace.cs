using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ChatSpace(string alias, string password, Guid[] userIds)
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public string Alias { get; set; } = alias;

        public string Password { get; set; } = password;

        public Guid[] UserIds { get; set; } = userIds;

        public ChatSpace() : this("An empty room", "allAreWelcome", [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()]) { }

        public static explicit operator ChatSpace(ChatSpacePost post) => new(
            post.Alias, post.Password, post.UserIds
        );
    }
}