using Telegram.Bot;

namespace TelegramBotTemplate.BackgroundServices;

public class PeriodicalCheckService : BackgroundService
{
    private readonly ILogger<PeriodicalCheckService> _logger;
    private readonly ITelegramBotClient _botClient;

    public PeriodicalCheckService(ILogger<PeriodicalCheckService> logger, ITelegramBotClient botClient)
    {
        _logger = logger;
        _botClient = botClient;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
    }
}