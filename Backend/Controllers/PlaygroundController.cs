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