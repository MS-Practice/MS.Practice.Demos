using Lib.AspNetCore.ServerSentEvents;
using System.Security.Cryptography;

namespace ServerSendEvent.HostServices
{
    public class ServerEventsWorker : BackgroundService
    {
        private readonly IServerSentEventsService client;

        public ServerEventsWorker(IServerSentEventsService client)
        {
            this.client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var clients = client.GetClients();
                    if (clients.Any())
                    {
                        var number = RandomNumberGenerator.GetInt32(1, 100);
                        await client.SendEventAsync(
                            //"123123123",
                            new ServerSentEvent
                            {
                                Id = "number",
                                Type = "chatsession",
                                Data = new List<string>
                                {
                                    $"Demo.AspNetCore.ServerSentEvents Heartbeat ({DateTime.UtcNow} UTC)"
                                }
                            },
                            stoppingToken
                        );
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}
