namespace EdgeHub
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IInitializationData _initialization;

        public Worker(ILogger<Worker> logger, IInitializationData initializationData)
        {
            _initialization=initializationData;
            _logger = logger;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("初始化数据信息");
            _initialization.Initialization();
            _logger.LogInformation("创建Mqtt客户端");
            _logger.LogInformation("创建Mqtt服务端");
            _initialization.CretaServerService();
            _logger.LogInformation("链接EMQX");
            await _initialization.CretaEmqxLink();
            Console.WriteLine("启动成功");
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("停止工作");
            return Task.CompletedTask;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

    }
}