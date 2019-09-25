using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using NLog.Config;
using NLog.Targets;

namespace NLog.Targets.AzureEventHubs
{
    [Target("AzureEventHub")]
    public class AzureEventHubTarget : TargetWithLayout
    {
        EventHubClient eventHubClient;

        [RequiredParameter]
        public string EventHubsConnectionString { get; set; }

        [RequiredParameter]
        public string EventHubPath { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            WriteAsync(logEvent).GetAwaiter().GetResult();
            //SendMessagesToEventHub("LOGMESSAGE");
        }

        private async Task WriteAsync(LogEventInfo logEvent)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubsConnectionString)
            {
                EntityPath = EventHubPath
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(logEvent);
            await eventHubClient.CloseAsync();
        }
        private async Task SendMessagesToEventHub(LogEventInfo logEvent)
        {
            string logMessage = this.Layout.Render(logEvent);

            try
            {
                //Sending message
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(logMessage)));
            }
            catch (Exception exception)
            {
                string ex = exception.ToString();
            }
            await Task.Delay(10);
            //messages sent
        }
    }
}
