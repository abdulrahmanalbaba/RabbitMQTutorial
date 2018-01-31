using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Publisher
{
    public partial class Publish : Form
    {
        public Publish()
        {
            InitializeComponent();

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtExchange.Text) || string.IsNullOrEmpty(txtMessage.Text) || string.IsNullOrEmpty(txtRoutingKey.Text))
            {
                MessageBox.Show("Fill in the fields dumb ass !!! this is just a POC :)");
            }
            else
            {
                string exchange = txtExchange.Text;
                string routingKey = txtRoutingKey.Text;
                string message = txtMessage.Text;
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange, type: "topic");
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: body);
                    this.listBoxPublishedMessages.Items.Add(string.Format("Sent '{0}' to route '{1}' on exchange'{2}'", message, routingKey, exchange));
                }
            }
        }
    }
}
