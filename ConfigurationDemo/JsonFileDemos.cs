using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;

namespace ConfigurationDemo
{
    [Trait("Category", "Configuration / JSON File")]
    public class JsonFileDemos
    {
        [Fact(DisplayName = "Process existing JSON File")]
        public void ProcessExistingJsonFile()
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile(@"TestData\Test.Main.json");
            
            var config = builder.Build();
            
            Assert.Equal("True", config["Verbose"]);
        }

        [Fact(DisplayName = "Process non-existing JSON File")]
        public void ProcessNonExistingJsonFile()
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile(@"TestData\Test.Nonsense.json");

            Assert.Throws<FileNotFoundException>(() => builder.Build());
        }

        [Fact(DisplayName = "Process optional JSON File")]
        public void ProcessOptionalJsonFile()
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile(@"TestData\Test.Nonsense.json", true);

            var config = builder.Build();

            Assert.Null(config["Verbose"]);
        }
    }
}
