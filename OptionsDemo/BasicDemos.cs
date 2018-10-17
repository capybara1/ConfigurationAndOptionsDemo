using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OptionsDemo
{
    [Trait("Category", "Options / Basics")]
    public class BasicDemos
    {
        private static readonly IDictionary<string, string> DefaultValues = new Dictionary<string, string>
        {
            ["App:Verbose"] = "true",
            ["App:Depth"] = "5",
            ["App:Output"] = "filepath",
        };

        [Fact(DisplayName = "Get static options")]
        public void GetStaticOptions()
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddInMemoryCollection(DefaultValues);

            var config = configurationBuilder.Build();
            var section = config.GetSection("App");

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppOptions>(section);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var optionsAccessor = serviceProvider.GetRequiredService<IOptions<AppOptions>>();
            var options = optionsAccessor.Value;

            Assert.True(options.Verbose);
            Assert.False(options.Silent);
        }

        [Fact(DisplayName = "Get dynamic options with a monitor")]
        public void GetDynamicOptionsWithOptionsMonitor()
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddInMemoryCollection(DefaultValues);

            var config = configurationBuilder.Build();
            var section = config.GetSection("App");

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppOptions>(section);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var optionsAccessor = serviceProvider.GetRequiredService<IOptionsMonitor<AppOptions>>();
            var options = optionsAccessor.CurrentValue;

            Assert.True(options.Verbose);

            config.Providers.Single().Set("App:Verbose", "false");
            config.Reload();
            
            options = optionsAccessor.CurrentValue;

            Assert.False(options.Verbose);
        }

        [Fact(DisplayName = "Get dynamic options with a snapshot")]
        public void GetDynamicOptionsWithSnapshot()
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddInMemoryCollection(DefaultValues);

            var config = configurationBuilder.Build();
            var section = config.GetSection("App");

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppOptions>(section);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            using (var scope = serviceProvider.CreateScope())
            {
                var optionsAccessor = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<AppOptions>>();
                var options = optionsAccessor.Value;

                Assert.True(options.Verbose);
            }
            
            config.Providers.Single().Set("App:Verbose", "false");
            config.Reload();
            
            using (var scope = serviceProvider.CreateScope())
            {
                var optionsAccessor = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<AppOptions>>();
                var options = optionsAccessor.Value;

                Assert.False(options.Verbose);
            }
        }
    }
}
