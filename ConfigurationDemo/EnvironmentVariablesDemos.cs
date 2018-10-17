using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

namespace ConfigurationDemo
{
    [Trait("Category", "Configuration / Environment Variables")]
    public class EnvironmentVariablesDemos
    {
        private static readonly IDictionary<string, string> DefaultValues = new Dictionary<string, string>
        {
            ["App:Verbose"] = "false",
            ["App:Depth"] = "1",
            ["App:Silent"] = "false",
        };

        [Fact(DisplayName = "Process Environment Variables")]
        public void ProcessEnvironmentVariables()
        {
            var builder = new ConfigurationBuilder();

            builder.AddInMemoryCollection(DefaultValues);

            System.Environment.SetEnvironmentVariable("App:Verbose", "true");

            builder.AddEnvironmentVariables();

            var config = builder.Build();

            var section = config.GetSection("App");
            Assert.Equal("true", section["Verbose"]);
        }
    }
}
