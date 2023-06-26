using MQTTnet;
using MQTTnet.Client;

namespace MQTTPublisher
{
    internal class Publisher
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            var client = mqttFactory.CreateMqttClient();

            try
            {
                var options = new MqttClientOptionsBuilder()
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithTcpServer("localhost", 1883)
                    .WithCleanSession()
                    .Build();

                await client.ConnectAsync(options);

                Console.WriteLine("按任何鍵publish message");
                Console.ReadLine();

                await PublishMessageAsync(client);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //await client.DisconnectAsync();
            }
        }

        /// <summary>
        /// 非同步發佈訊息
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task PublishMessageAsync(IMqttClient client)
        {
            string messagePayload = "Hello";

            string messagePayload1 = messagePayload + "1";
            var msg1 = new MqttApplicationMessageBuilder()
                .WithTopic("home/workStation1")
                .WithPayload(messagePayload1)
                .Build();

            string messagePayload2 = messagePayload + "2";
            var msg2 = new MqttApplicationMessageBuilder()
                .WithTopic("home/workStation2")
                .WithPayload(messagePayload2)
                .Build();

            if (client.IsConnected)
            {
                await client.PublishAsync(msg1);
                await client.PublishAsync(msg2);

                Console.WriteLine($"published Message-{messagePayload1}");
                Console.WriteLine($"published Message-{messagePayload2}");
            }
        }
    }
}