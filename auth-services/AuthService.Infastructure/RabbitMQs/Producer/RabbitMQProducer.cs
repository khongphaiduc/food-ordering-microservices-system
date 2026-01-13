using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using auth_services.AuthService.Infastructure.IntegrationContracts;

namespace auth_services.AuthService.Infastructure.RabbitMQs.Producer
{
    public class RabbitMQProducer
    {
        private readonly IConfiguration _iConfig;

   
        public RabbitMQProducer(IConfiguration configuration)
        {
            _iConfig = configuration;
        }


        public async Task SendMessage(UserInfoDTO message, string routingKey)
        {

            var factory = new ConnectionFactory
            {

                HostName = _iConfig["RabbitMQ_Side_Auth:Host"]!,
                UserName = _iConfig["RabbitMQ_Side_Auth:UserName"]!,
                Password = _iConfig["RabbitMQ_Side_Auth:Password"]!,
                ClientProvidedName = "AuthService"
            };


            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();


            //khai báo Exchange
            await channel.ExchangeDeclareAsync(
                exchange: _iConfig["RabbitMQ_Side_Auth:Exchange"]!,
                type: ExchangeType.Direct,
                durable: true                              // ghi exchange xuống disk  ,  nếu sau này mà RabbitMQ bị tắt thì exchange vẫn còn , không cần tạo lại 
            );


            // khai báo queue
            await channel.QueueDeclareAsync(
             queue: _iConfig["RabbitMQ_Side_Auth:Queue:Notification_Email:QueueName"]!,
             durable: true,                              // ghi queue xuống disk  ,  nếu sau này mà RabbitMQ bị tắt thì  queue vẫn còn , không cần tạo lại 
             exclusive: false,
             autoDelete: false
             );




            await channel.QueueDeclareAsync(
            queue: _iConfig["RabbitMQ_Side_Auth:Queue:UserInfo:QueueName"]!,
            durable: true,                              // ghi queue xuống disk  ,  nếu sau này mà RabbitMQ bị tắt thì  queue vẫn còn , không cần tạo lại 
            exclusive: false,
            autoDelete: false
            );


            // map(bind) queue với exchange
            await channel.QueueBindAsync(
             queue: _iConfig["RabbitMQ_Side_Auth:Queue:Notification_Email:QueueName"]!,
             exchange: _iConfig["RabbitMQ_Side_Auth:Exchange"]!,
             routingKey: _iConfig["RabbitMQ_Side_Auth:Queue:Notification_Email:RoutingKey"]!
            );


            await channel.QueueBindAsync(
            queue: _iConfig["RabbitMQ_Side_Auth:Queue:UserInfo:QueueName"]!,
            exchange: _iConfig["RabbitMQ_Side_Auth:Exchange"]!,
            routingKey: _iConfig["RabbitMQ_Side_Auth:Queue:UserInfo:RoutingKey"]!
           );

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);


            // các thuộc tính của message
            var properties = new BasicProperties
            {
                Persistent = true,     // ghi xuống disk 
                Headers = new Dictionary<string, object>
                {
                    { "producer", "Auth-Service" },
                    { "service", "Auth-Service" },
                    { "message_type", "SendEmail" },
                    { "created_at", DateTime.UtcNow.ToString("O") }
                }
            };



            // push message vào RabbitMQ 
            await channel.BasicPublishAsync(
                exchange: _iConfig["RabbitMQ_Side_Auth:Exchange"]!,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body
            );

        }

    }


    // exchange sẽ dựa trên RoutingKey để chuyền message đến đúng queue cần thiết
}
