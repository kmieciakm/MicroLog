using MicroLog.Collector.Client;
using MicroLog.Collector.Tests.Fixture;
using MicroLog.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MicroLog.Collector.Tests
{
    public class CollectorCases : CollectorFixture
    {
        [Fact]
        public void MicroLog_CanPushLog()
        {
            using var services = Services().Start();
            Task.Delay(15000).GetAwaiter().GetResult(); // Wait for all services to initialize

            MicroLogConfig config = GetMicroLogConfig(services);
            MicroLogClient client = new MicroLogClient(config);

            client.LogInformationAsync("Test message").GetAwaiter().GetResult();
        }
    }
}
