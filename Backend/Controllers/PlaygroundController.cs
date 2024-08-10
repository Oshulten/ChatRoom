using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaygroundController : ControllerBase
    {
        [HttpGet("now")]
        public DateTime GetDate()
        {
            return DateTime.Now;
        }

        [HttpGet("list-of-ints")]
        public List<int> GetListOfInts()
        {
            return [1, 2, 3, 4];
        }

        [HttpGet("dictionary-string-int")]
        public Dictionary<string, int> GetDictionaryOfStringInt()
        {
            return new Dictionary<string, int>()
            {
                ["a"] = 1,
                ["b"] = 2,
            };
        }

        [HttpGet("chat-user")]
        public ChatUser GetChatUser()
        {
            return new ChatUser("Fredrick", "my-password", true, new DateTime(2023, 5, 5));
        }

        public record RecordWithDate(DateTime date);
        [HttpGet("record-with-date")]
        public RecordWithDate GetRecordWithDate()
        {
            return new RecordWithDate(new DateTime(2023, 5, 5));
        }

        public class ComplexComponent(string message, bool isComplex)
        {
            public string Message { get; set; } = message;
            public bool IsComplex { get; set; } = isComplex;
        }

        public class ComplexClass
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public List<DateTime> ListOfDates { get; set; } = [
                new DateTime(2023, 4, 9),
                new DateTime(1882, 2, 5),
                new DateTime(500, 2, 9),
            ];
            public Dictionary<string, Dictionary<int, ComplexComponent>> DictionaryOfDictionaries { get; set; } = new()
            {
                ["firstDictionary"] = new()
                {
                    [1] = new ComplexComponent("Complex message", true),
                    [-1] = new ComplexComponent("Simple message", false),
                },
                ["secondDictionary"] = new()
                {
                    [23] = new ComplexComponent("Monkeys are worse than donkeys", false),
                    [5] = new ComplexComponent("I don't know half of you half as well as I should like; and I like less than half of you half as well as you deserve.", true),
                }
            };
        }

        [HttpGet("complex-class")]
        public ComplexClass GetComplexClass()
        {
            ComplexClass sample = new ComplexClass();
            return sample;
        }
    }
}