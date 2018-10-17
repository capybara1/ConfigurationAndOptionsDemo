using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

namespace ConfigurationDemo
{
    [Trait("Category", "Configuration / Command Line")]
    public class CommandLineDemos
    {
        private static readonly IDictionary<string, string> DefaultValues = new Dictionary<string, string>
        {
            ["App:Verbose"] = "false",
            ["App:Depth"] = "1",
            ["App:Silent"] = "false",
        };

        private static readonly IDictionary<string, string> SwitchMappings = new Dictionary<string, string>
        {
            ["-d"] = "App:Depth",
        };

        [Fact(DisplayName = "Process Command Line Arguments")]
        public void ProcessCommandLineArguments()
        {
            var builder = new ConfigurationBuilder();

            builder.AddInMemoryCollection(DefaultValues);

            var args = new[] { "App:Verbose=true", "-d", "5", "/app:silent", "true" };
            builder.AddCommandLine(args, SwitchMappings);

            var config = builder.Build();

            var section = config.GetSection("App");
            Assert.Equal("true", section["Verbose"]);
            Assert.Equal("5", section["Depth"]);
            Assert.Equal("true", section["Silent"]);
        }
    }
}
