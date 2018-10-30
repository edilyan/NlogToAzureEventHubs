using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace DataSenderToEventHub
{
    public static class SenderToEventHub
    {
        private static EventHubClient _eventHubClient;
        private static string eventHubConnectionString = "Endpoint=sb://domlogeventhubsns.servicebus.windows.net/;SharedAccessKeyName=Manage;SharedAccessKey=RhkR2Q6oqU4FczgkJY82BnYwI8kJq7hzX6tyXPq65NM=;EntityPath=domlogamqp";

        static SenderToEventHub() => _eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString);

        public static  async Task DataSenderAsync(string data)
        {
            var dataAsJson = JsonConvert.SerializeObject(data);
            var eventData = new EventData(Encoding.UTF8.GetBytes(dataAsJson));
            await _eventHubClient.SendAsync(eventData);
        }
    }

}
