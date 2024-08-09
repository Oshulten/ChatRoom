using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatMessagesController(ChatroomDatabaseContext db) : ControllerBase
    {
        [HttpGet("{id}")]
        public ChatMessage GetById(Guid id)
        {
            return db.ChatMessages.FirstOrDefault(data => data.Id == id)!;
        }

        [HttpGet]
        public IEnumerable<ChatMessage> GetAll()
        {
            return db.ChatMessages;
        }

        [HttpGet("after-date/{date}")]
        public IEnumerable<ChatMessage> GetAfterDate(DateTime date)
        {
            return db.ChatMessages.Where(message => message.PostedAt.CompareTo(date) > 0)
                                  .OrderBy(message => message.PostedAt);
        }

        [HttpPost]
        public ActionResult Post(ChatMessagePost post)
        {
            var message = (ChatMessage)post;
            db.ChatMessages.Add(message);
            db.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = message.Id }, null);
        }
    }
}