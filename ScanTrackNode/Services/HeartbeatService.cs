namespace ScanTrackNode.Services;

public class HeartbeatService(NodeRegistry registry) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            await registry.RegisterSelfAsync();
        }
    }

    public Task ForceHeartbeatAsync(CancellationToken stoppingToken)
        => registry.RegisterSelfAsync();
}