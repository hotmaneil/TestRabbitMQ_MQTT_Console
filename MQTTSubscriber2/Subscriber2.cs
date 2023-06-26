using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace MQTTSubscriber2
{
    internal class Subscriber2
    {
        static async Task Main(string[] args)
        {
            try
            {
                var mqttFactory = new MqttFactory();
                IMqttClient client = mqttFactory.CreateMqttClient();
                var options = new MqttClientOptionsBuilder()
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithTcpServer("localhost", 1883)
                    .WithCleanSession()
                    .Build();

                client.ConnectedAsync += _mqttClient_ConnectedAsync;
                client.ApplicationMessageReceivedAsync += _mqttClient_ApplicationMessageReceivedAsync;

                var topicFilter = new MqttTopicFilterBuilder()
                   .WithTopic("home/workStation2")
                   .Build();

                await client.ConnectAsync(options);
                await client.SubscribeAsync(topicFilter);

                Console.ReadLine();

                await client.DisconnectAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task _mqttClient_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            Console.WriteLine("subscriber2連到broker成功！");
            await Task.CompletedTask;
        }

        public static async Task _mqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            var topic = arg.ApplicationMessage.Topic;
            var payloadText = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);

            Console.WriteLine($"subscriber2 Received: Topic:{topic}, Payload:{payloadText}");
            await Task.CompletedTask;
        }
    }
}