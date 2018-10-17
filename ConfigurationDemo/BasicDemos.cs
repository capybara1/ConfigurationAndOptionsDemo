using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

namespace ConfigurationDemo
{
    [Trait("Category", "Configuration / Basics")]
    public class BasicDemos
    {
        private static readonly IDictionary<string, string> DefaultValues = new Dictionary<string, string>
        {
            ["App:Verbose"] = "true",
            ["App:Depth"] = "5",
            ["App:Output"] = "filepath",
        };

        [Fact(DisplayName = "Add Single Configuration Provider")]
        public void AddSingleConfigurationProvider()
        {
            var builder = new ConfigurationBuilder();

            builder.AddInMemoryCollection(DefaultValues);

            var config = builder.Build();

            var section = config.GetSection("App");

            Assert.Equal("true", section["Verbose"]);
            Assert.Equal("5", section["Depth"]);
            Assert.Equal("filepath", section["Output"]);
        }
        
        [Fact(DisplayName = "Add Cascade of Configuration Providers")]
        public void AddCascadeOfConfigurationProviders()
        {
            var builder = new ConfigurationBuilder();

            builder.AddInMemoryCollection(DefaultValues);

            var overrides = new Dictionary<string, string>
            {
                ["App:Depth"] = "4",
            };

            builder.AddInMemoryCollection(overrides);

            var config = builder.Build();

            var section = config.GetSection("App");

            Assert.Equal("true", section["Verbose"]);
            Assert.Equal("4", section["Depth"]);
            Assert.Equal("filepath", section["Output"]);
        }
    }
}
