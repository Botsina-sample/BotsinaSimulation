using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;

namespace MainBroker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        MqttBroker broker = new MqttBroker();
        private void button1_Click(object sender, EventArgs e)
        {
            broker.Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            broker.Stop();

            base.OnClosed(e);
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
