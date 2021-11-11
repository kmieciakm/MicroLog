using MicroLog.Collector.Client;
using MicroLog.FunctionalTests.Collector.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MicroLog.FunctionalTests.Collector.Cases
{
    public class CollectorCases : CollectorFixture
    {
        [Fact]
        public void MicroLog_CanPushLog()
        {
            using var services = Services().Start();
            Task.Delay(15000).GetAwaiter().GetResult(); // Wait for all services to initialize

            LogCollectorConfig config = GetMicroLogConfig(services);
            LogCollectorClient client = new LogCollectorClient(config);

            client.LogInformationAsync("Test message").GetAwaiter().GetResult();
        }
    }
}
