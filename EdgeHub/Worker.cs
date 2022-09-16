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
            _logger.LogInformation("��ʼ��������Ϣ");
            _initialization.Initialization();
            _logger.LogInformation("����Mqtt�ͻ���");
            _logger.LogInformation("����Mqtt�����");
            _initialization.CretaServerService();
            _logger.LogInformation("����EMQX");
            await _initialization.CretaEmqxLink();
            Console.WriteLine("�����ɹ�");
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ֹͣ����");
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