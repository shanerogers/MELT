using MELT;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace SampleWebApplication.Tests
{
    public class LoggingTestWithInjectedFactoryWithoutHelpers : IClassFixture<CustomWebApplicationFactoryWithoutHelpers<Startup>>
    {
        private readonly CustomWebApplicationFactoryWithoutHelpers<Startup> _factory;

        public LoggingTestWithInjectedFactoryWithoutHelpers(CustomWebApplicationFactoryWithoutHelpers<Startup> factory)
        {
            _factory = factory;
            // In this case the factory will be resused for all tests, so the sink will be shared as well.
            // We can clear the sink before each test execution, as xUnit will not run this tests in parallel.
            _factory.Sink.Clear();
        }

        [Fact]
        public async Task ShouldLogHelloWorld()
        {
            // Arrange  

            // Act
            await _factory.CreateDefaultClient().GetAsync("/");

            // Assert
            var log = Assert.Single(_factory.Sink.LogEntries);
            // Assert the message rendered by a default formatter
            Assert.Equal("Hello World!", log.Message);
        }

        [Fact]
        public async Task ShouldUseScope()
        {
            // Arrange  

            // Act
            await _factory.CreateDefaultClient().GetAsync("/");

            // Assert
            var log = Assert.Single(_factory.Sink.LogEntries);
            // Assert the scope rendered by a default formatter
            Assert.Equal("I'm in the GET scope", log.Scope.Message);
        }
    }

    public class CustomWebApplicationFactoryWithoutHelpers<TStartup> : WebApplicationFactory<TStartup>
         where TStartup : class
    {
        public ITestSink Sink { get; } = MELTBuilder.CreateTestSink(options => options.FilterByNamespace(nameof(SampleWebApplication)));

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging => logging.AddTestLogger(Sink));
        }
    }
}
