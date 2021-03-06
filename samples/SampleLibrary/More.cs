using Microsoft.Extensions.Logging;

namespace SampleLibrary
{
    public class More
    {
        private readonly ILogger<More> _logger;

        public Sample Sample { get; }

        public More(Sample sample, ILogger<More> logger)
        {
            Sample = sample;
            _logger = logger;
        }

        public void DoMore()
        {
            _logger.LogInformation("More is less.");
            Sample.DoSomething();
        }
    }
}
