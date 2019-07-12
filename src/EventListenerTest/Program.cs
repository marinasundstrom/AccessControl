namespace ReadDeviceToCloudMessages
{
    using System;
    using System.Threading.Tasks;
    using System.Text;
    using System.Threading;
    using System.Linq;
    using Microsoft.Azure.EventHubs;

    public class Program
    {
        private const string ConnectionString = "Endpoint=sb://ihsuproddbres005dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=TtyeV4qhWAeWsOmrkazNCzR9NUF2I9WsarrGFdB2Zn8=;EntityPath=iothub-ehub-accesscont-1881175-6c91e95a58";
        private static EventHubClient _eventHubClient;

        private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken cancellationToken)
        {
            var eventHubReceiver = _eventHubClient.CreateReceiver(PartitionReceiver.DefaultConsumerGroupName, partition, EventPosition.FromEnd());
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) break;
                var eventData = await eventHubReceiver.ReceiveAsync(1);
                if (eventData == null) continue;

                foreach(var d in eventData)
                {
                    var data = Encoding.UTF8.GetString(d.Body);
                    Console.WriteLine($"{d.SystemProperties["iothub-connection-device-id"]}: {data}");
                    //Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);
                }
            }
        }

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Receive messages. Ctrl-C to exit.\n");
            _eventHubClient = EventHubClient.CreateFromConnectionString(ConnectionString);
            var info = await _eventHubClient.GetRuntimeInformationAsync();
            var partitions = info.PartitionIds;
            var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting...");
            };

            var tasks = partitions.Select(partition => ReceiveMessagesFromDeviceAsync(partition, cts.Token));
            Task.WaitAll(tasks.ToArray());
        }
    }
}