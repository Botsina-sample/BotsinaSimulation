using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

// For Mqtt Communication
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

// For spy and playback functions
using Gu.Wpf.UiAutomation;

// Get current IP address
using System.Net;
using System.Windows.Threading;
using System.Diagnostics;
using BotsinaAgent.Actions;

// DEFAULT UiAutomation by Microsoft
using System.Windows.Automation;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

using BotsinaAgent.Spy;

// Write json result
using Newtonsoft.Json;

namespace BotsinaAgent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SR.username = Environment.UserName;

            string GUWPF = @"C:\Users\" + SR.username + @"\Documents\GUWPF";
            string SpyIMG = @"C:\Users\" + SR.username + @"\Documents\GUWPF\SpyIMG";

            if (!Directory.Exists(GUWPF))
            {
                Directory.CreateDirectory(GUWPF);
                Directory.CreateDirectory(SpyIMG);

            }
        }



        #region Global Variables
        Process[] flexProc = Process.GetProcessesByName("AUT_SampleUI");
        Gu.Wpf.UiAutomation.Application App;
        IReadOnlyList<UiElement> Element;
        Gu.Wpf.UiAutomation.Window MainWindow;
        Control control = new Control();
        List<Control> Controllist_copy = new List<Control>();

        SpyResult SR = new SpyResult();
        

        MqttClient client;
        string clientId;
        string Topic = "BotsinaDC";
        string myIP = null;
        int id;
        #endregion

        #region DllImport
        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 6;
        // more here: http://www.pinvoke.net/default.aspx/user32.showwindow
        #endregion

        #region UiElement Functions
        public IReadOnlyList<UiElement> ElementClass(string type)
        {
            return MainWindow.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, type));
        }
        public IReadOnlyList<UiElement> SearchbyFramework(string type)
        {
            return MainWindow.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.FrameworkIdProperty, type));
        }
        #endregion

        public string GetCurrentIP()
        {
            string hostName = Dns.GetHostName();
            return myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                client = new MqttClient(GetCurrentIP());
                
                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

                clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);

                client.Subscribe(new string[] { Topic }, new byte[] { 2 });   // we need arrays as parameters because we can subscribe to different topics with one call
                listBox1.Items.Add("Connected to " + Topic);
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.Message);
            }
        }

        protected override void OnClosed(EventArgs e)
        {

            //Do something 
            client.Disconnect();

            //////////////////

            base.OnClosed(e);
            System.Windows.Forms.Application.Exit();
        }


        // STOP BUTTON - DISCONNECT FROM DC BROKER
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                client.Disconnect();
                base.OnClosed(e);
                System.Windows.Forms.Application.Exit(); 
            } catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            //Thread.Sleep(1000);

            Dispatcher.CurrentDispatcher.Invoke(delegate {
                if(ReceivedMessage == "REQ_SPY")
                {
                    //Spy();
                    System.Windows.Forms.MessageBox.Show("MOAISJDO");
                } 
                if (ReceivedMessage == "REQ_PLAYBACK")
                {
                    //DoPlayBack();
                    
                }

            });

        }

        private void SendMess(string command)
        {
            client.Publish(Topic, Encoding.UTF8.GetBytes(command), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        private void deletefiles()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(@"C:\Users\" + SR.username + @"\Documents\GUWPF\SpyIMG");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        // SPY FUNCTION
        public void Spy()
        {
            IntPtr hWnd = flexProc[0].MainWindowHandle;
            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
                ShowWindow(hWnd, SW_MAXIMIZE);
            }
            App = Gu.Wpf.UiAutomation.Application.Attach(flexProc[0].Id);
            MainWindow = App.MainWindow;

            Element = MainWindow.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.FrameworkIdProperty, "WPF"));
            try
            {

                //Clear SpyIMG files
                deletefiles();

                var curtime = DateTime.Now;
                var day = curtime.Day;
                var month = curtime.Month;
                var year = curtime.Year;

                var sec = curtime.Second;
                var hour = curtime.Hour;
                var minute = curtime.Minute;
                var longTimeString = DateTime.Now.ToOADate();


                var reformat = day + "-" + month + "-" + year + "__" + hour + "-" + minute + "-" + sec + "-";


                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\" + SR.username + @"\Documents\GUWPF\" + reformat + "SpyResult.json"))
                {
                    this.WindowState = FormWindowState.Minimized;
                    Thread.Sleep(500);
                    SR.currenttime = longTimeString.ToString();

                    id = 0;
                    foreach (UiElement UIE in Element)
                    {
                        if (!UIE.Bounds.IsEmpty)
                        {
                            SR.index = id;
                            SR.autoDI = UIE.AutomationId;
                            SR.classname = UIE.ClassName;
                            SR.name = UIE.Name;
                            SR.directory = @"C:\Users\" + SR.username + @"\Documents\GUWPF\SpyIMG\" + SR.index + " - " + SR.classname + ".png";
                            //SR.currenttime = curtime.ToString();


                            if (UIE.AutomationId == "")
                                SR.autoDI = "No AutomationID";
                            if (UIE.Name == "")
                                SR.name = "No Name";

                            UIE.CaptureToFile(@"C:\Users\" + SR.username + @"\Documents\GUWPF\SpyIMG\" + SR.index + " - " + SR.classname + ".png");
                            id++;



                            listBox1.Items.Add("ID: " + SR.index + " - " + SR.autoDI + " - " + SR.classname + " - " + SR.name);
                            string ObjectUI = JsonConvert.SerializeObject(SR, Newtonsoft.Json.Formatting.Indented);

                            if (SR.index == 0)
                            {
                                file.WriteLine("[");
                                file.WriteLine(ObjectUI + ",");
                            }
                            else if (SR.index == Element.Count - 1)
                            {
                                file.WriteLine(ObjectUI);
                                file.WriteLine("]");
                            }

                            else file.WriteLine(ObjectUI + ",");

                        }

                    }
                    file.WriteLine("]");
                    this.WindowState = FormWindowState.Normal;
                }
            }
            catch (Exception err)
            {
                listBox1.Items.Add(id + " - " + SR.classname + " - " + err.Message);
            }

        }

        public void GetAUTProc()
        {
            IntPtr hWnd = flexProc[0].MainWindowHandle;
            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
                ShowWindow(hWnd, SW_MAXIMIZE);
            }

            App = Gu.Wpf.UiAutomation.Application.Attach(flexProc[0].Id);
            MainWindow = App.MainWindow;
            Element = SearchbyFramework("WPF");
            Console.WriteLine("AUT's ProcessId: " + MainWindow.ProcessId);
        }

        public void DoPlayBack()
        {
            listBox1.Items.Add("RES_PLAYBACK");
            SendMess("RES_PLAYBACK");
        }

    }
}
