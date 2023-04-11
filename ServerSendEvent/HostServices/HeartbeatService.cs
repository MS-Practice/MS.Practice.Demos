using Lib.AspNetCore.ServerSentEvents;

namespace ServerSendEvent.HostServices
{
    public class HeartbeatService : BackgroundService
    {
        private const string HEARTBEAT_MESSAGE_FORMAT = "Demo.AspNetCore.ServerSentEvents Heartbeat ({0} UTC)";
        private readonly IServerSentEventsService _serverSentEventsService;
        public HeartbeatService(IServerSentEventsService serverSentEventsService)
        {
            _serverSentEventsService = serverSentEventsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _serverSentEventsService.SendEventAsync(String.Format(HEARTBEAT_MESSAGE_FORMAT, DateTime.UtcNow), stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
