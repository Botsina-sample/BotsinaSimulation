using System;
using System.Net;
using System.Text;
using System.Threading;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DCSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MqttClient client;
        string clientId;
        string Topic = "BotsinaDC";
        string myIP = null;
        

        public MainWindow()
        {
            InitializeComponent();
        }



        public string GetCurrentIP()
        {
            string hostName = Dns.GetHostName();
            return myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
        }

        protected override void OnClosed(EventArgs e)
        {

            //Do something 
            client.Disconnect();

            //////////////////

            base.OnClosed(e);
            App.Current.Shutdown();
        }

        //Connect button
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new MqttClient(GetCurrentIP());

                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

                clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);
                
                client.Subscribe(new string[] { Topic }, new byte[] { 2 });   // we need arrays as parameters because we can subscribe to different topics with one call
                listBox.Items.Add("Connected to: " + Topic);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            //Thread.Sleep(1000);
        
            Dispatcher.Invoke(delegate {

                if(ReceivedMessage == "RES_SPY")
                {
                    listBox.Items.Add("RES_SPY");
                }
                else if(ReceivedMessage == "RES_PLAYBACK")
                {
                    listBox.Items.Add("RES_PLAYBACK");
                }
       

            });
        }


        private void sendcommand(string command)
        {
            client.Publish(Topic, Encoding.UTF8.GetBytes(command), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        //Spy Button
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            sendcommand("REQ_SPY");
        }

   
        // Playback Button
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string json = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            sendcommand(json);
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
