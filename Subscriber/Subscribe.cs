using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Windows.Forms;

namespace Subscriber
{
    public partial class Subscribe : Form
    {
        public Subscribe()
        {
            InitializeComponent();

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtExchange.Text) || string.IsNullOrEmpty(txtRoutingKey.Text))
            {
                MessageBox.Show("Fill in the fields dumb ass !!! this is just a POC :)");
            }
            else
            {
                string exchange = txtExchange.Text;
                string topic = txtRoutingKey.Text;

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange, type: "topic");
                    var queueName = channel.QueueDeclare().QueueName;

                    foreach (var bindingKey in topic.Split(','))
                    {
                        channel.QueueBind(queue: queueName, exchange: exchange, routingKey: bindingKey);
                    }

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += Consumer_Received;
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                }
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            var routingKey = e.RoutingKey;
            this.Invoke(new MethodInvoker(delegate () { listBoxReceivedMessages.Items.Add(string.Format(" [x] Received '{0}' from topic'{1}'", message, routingKey)); }));
        }
    }
}
