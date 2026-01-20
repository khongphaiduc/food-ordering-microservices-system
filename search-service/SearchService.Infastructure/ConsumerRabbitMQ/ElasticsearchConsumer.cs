
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using search_service.SearchService.Application.Interface;
using System.Text;
using System.Text.Json;

namespace search_service.SearchService.Infastructure.ConsumerRabbitMQ
{
    public class ElasticsearchConsumer : BackgroundService
    {
        private readonly IConfiguration _iconfig;
        private readonly ILogger<ElasticsearchConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection _connection;
        private IChannel _channel;
        public ElasticsearchConsumer(IConfiguration configuration, ILogger<ElasticsearchConsumer> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _iconfig = configuration;
            _logger = logger;
            _scopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _iconfig["RabbitMQ_Side_SearchService:Host"]!,
                    UserName = _iconfig["RabbitMQ_Side_SearchService:Username"]!,
                    Password = _iconfig["RabbitMQ_Side_SearchService:Password"]!,
                };

                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                await _channel.QueueDeclareAsync(
                      _iconfig["RabbitMQ_Side_SearchService:Queue:UpdateProduct:QueueName"]!,
                          durable: true,
                          exclusive: false,
                          autoDelete: false,
                          arguments: null
                );

                await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var elasticSearchUpdate =
                        scope.ServiceProvider.GetRequiredService<IElasticsearchUpdateDatabase>();

                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    try
                    {
                        if (!Guid.TryParse(message, out var id))
                        {
                            _logger.LogWarning("Invalid message: {Message}", message);


                            await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                            return;
                        }

                        await elasticSearchUpdate.UpdateDocumentFromDatabase(id);

                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Temporary error");
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                    }
                };



                await _channel.BasicConsumeAsync(
                 queue: _iconfig["RabbitMQ_Side_SearchService:Queue:UpdateProduct:QueueName"]!,
                 autoAck: false,
                 consumerTag: "ElasticsearchConsumer",
                 consumer: consumer
                );

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }

            }
            catch (Exception ex)
            {

                _logger.LogInformation($"Elasticsearch Consumer Bug : {ex.Message}");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UserInfoConsumer stopping...");

            try
            {
                if (_channel is not null)
                    await _channel.CloseAsync(cancellationToken);

                if (_connection is not null)
                    await _connection.CloseAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while stopping UserInfoConsumer");
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
