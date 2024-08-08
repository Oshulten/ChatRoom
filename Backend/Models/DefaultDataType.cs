using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class DefaultDataType(int value1, string value2)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public int Value1 { get; set; } = value1;
        public string Value2 { get; set; } = value2;

        // An explicit parameterless constructor is required for database creation
        public DefaultDataType() : this(default, string.Empty) { }

        public void Patch(DefaultDataType patchObject)
        {
            Value1 = patchObject.Value1;
            Value2 = patchObject.Value2;
        }

        public static List<DefaultDataType> SeedData(int numberOfDataPoints)
        {
            return Enumerable.Range(0, numberOfDataPoints)
                             .Select(i => new DefaultDataType(
                                Random.Shared.Next(-10, 10),
                                i.ToString()
                             )).ToList();
        }
    }
}