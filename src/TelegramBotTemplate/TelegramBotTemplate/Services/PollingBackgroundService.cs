 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; 

namespace TelegramBotTemplate.Services;


/// <summary>An abstract class to compose Polling background service and Receiver implementation classes</summary>
/// <remarks>See more: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services#consuming-a-scoped-service-in-a-background-task</remarks>
/// <typeparam name="TReceiverService">Receiver implementation class</typeparam>
public class PollingBackgroundService(IServiceProvider serviceProvider, ILogger<PollingBackgroundService> logger) : BackgroundService 
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting polling service");
        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        // Make sure we receive updates until Cancellation Requested
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Create new IServiceScope on each iteration. This way we can leverage benefits
                // of Scoped TReceiverService and typed HttpClient - we'll grab "fresh" instance each time
                using var scope = serviceProvider.CreateScope();
                var receiver = scope.ServiceProvider.GetRequiredService<ReceiverService>();
                await receiver.ReceiveAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError("Polling failed with exception: {Exception}", ex);
                // Cooldown if something goes wrong
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
