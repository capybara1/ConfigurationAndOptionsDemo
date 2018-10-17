using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ConfigurationDemo
{
    [Trait("Category", "Configuration / Model Binding")]
    public class ModelBindingDemos
    {
        private static readonly IDictionary<string, string> DefaultValues = new Dictionary<string, string>
        {
            ["App:Verbose"] = "true",
            ["App:Depth"] = "5",
            ["App:Output"] = "filepath",
        };

        [Fact(DisplayName = "Bind object to configuration")]
        public void BindObjectToConfiguration()
        {
            var builder = new ConfigurationBuilder();

            builder.AddInMemoryCollection(DefaultValues);

            var config = builder.Build();

            var section = config.GetSection("App");

            var appConfig = new AppConfig();
            section.Bind(appConfig);

            Assert.Equal(true, appConfig.Verbose);
            Assert.Equal(5, appConfig.Depth);
            Assert.Equal(false, appConfig.Silent);
            Assert.Equal("filepath", appConfig.Output);
        }
        
        [Fact(DisplayName = "Bound object does not respond to configuration reload")]
        public void BoundObjectDoesNotRespondToConfigurationReload()
        {
            var builder = new ConfigurationBuilder();

            builder.AddInMemoryCollection(DefaultValues);

            var config = builder.Build();

            var section = config.GetSection("App");

            var appConfig = new AppConfig();
            section.Bind(appConfig);

            Assert.Equal(true, appConfig.Verbose);
            Assert.Equal(5, appConfig.Depth);
            Assert.Equal(false, appConfig.Silent);
            Assert.Equal("filepath", appConfig.Output);

            config.Providers.Single().Set("App:Depth", "4");
            config.Reload();

            Assert.Equal("4", section["Depth"]);
            Assert.Equal(5, appConfig.Depth);
        }
    }
}
