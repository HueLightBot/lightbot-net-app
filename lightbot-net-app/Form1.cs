using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using System.Threading;
using Q42.HueApi.Converters;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Original;
using PubSub;
using ServiceStack.Redis;

namespace lightbot_net_app
{
    public partial class Form1 : Form
    {
        public IEnumerable<Q42.HueApi.Models.Bridge.LocatedBridge> bridgeIPs;
        public IBridgeLocator locator = new HttpBridgeLocator();
        public ILocalHueClient client;
        public IReadOnlyCollection<Q42.HueApi.Models.Groups.Group> groups;
        public System.Threading.Thread pubsubThread;
        public Form1()
        {
            InitializeComponent();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == false) {
                this.ShowDialog();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /// Checkboxes
            checkBox1.Checked = Properties.Settings.Default.cheering;
            checkBox2.Checked = Properties.Settings.Default.largeCheer;
            checkBox3.Checked = Properties.Settings.Default.onOff;
            checkBox4.Checked = Properties.Settings.Default.subs;

            /// int Textboxes
            textBox2.Text = Properties.Settings.Default.cheerFloor.ToString();
            textBox3.Text = Properties.Settings.Default.largeCheerFloor.ToString();
            textBox4.Text = Properties.Settings.Default.offFloor.ToString();
            textBox5.Text = Properties.Settings.Default.onFloor.ToString();

            /// Comboboxes
            comboBox1.Text = Properties.Settings.Default.largeCheerAction;
            comboBox2.SelectedText = Properties.Settings.Default.primeSubAction;
            comboBox3.Text = Properties.Settings.Default.Tier1SubAction;
            comboBox4.Text = Properties.Settings.Default.Tier2SubAction;
            comboBox5.Text = Properties.Settings.Default.Tier3SubAction;

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private async void button4_Click(object sender, EventArgs e)
        {
            bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(10));
            client = new LocalHueClient(bridgeIPs.First().IpAddress);
            if (Properties.Settings.Default.appkey != "")
            {
                client.Initialize(Properties.Settings.Default.appkey);
            } else
            {
                var appKey = await client.RegisterAsync("HueLightBot", Environment.MachineName);
                Properties.Settings.Default.appkey = appKey;
                Properties.Settings.Default.Save();
            }

            groups = await client.GetGroupsAsync();
            List<string> groupNames =  groups.Select(x => x.Name).ToList();

            comboBox6.DataSource = groupNames;
            
            //var command = new LightCommand();
            //command.Effect = Effect.None;
            //await client.SendCommandAsync(command, new List<string> { "4" });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /// Checkboxes
            Properties.Settings.Default.cheering = checkBox1.Checked;
            Properties.Settings.Default.largeCheer = checkBox2.Checked;
            Properties.Settings.Default.onOff = checkBox3.Checked;
            Properties.Settings.Default.subs = checkBox4.Checked;

            /// int Textboxes
            Properties.Settings.Default.cheerFloor = Int32.Parse(textBox2.Text);
            Properties.Settings.Default.largeCheerFloor = Int32.Parse(textBox3.Text);
            Properties.Settings.Default.offFloor = Int32.Parse(textBox4.Text);
            Properties.Settings.Default.onFloor = Int32.Parse(textBox5.Text);

            /// Comboboxes
            Properties.Settings.Default.largeCheerAction = comboBox1.Text;
            Properties.Settings.Default.primeSubAction = comboBox2.SelectedText;
            Properties.Settings.Default.Tier1SubAction = comboBox3.Text;
            Properties.Settings.Default.Tier2SubAction = comboBox4.Text;
            Properties.Settings.Default.Tier3SubAction = comboBox5.Text;

            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /// stop
            if (pubsubThread.IsAlive)
            {
                pubsubThread.Interrupt();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /// start
            if (pubsubThread == null)
            {
                pubsubThread = (new Thread(() => pubsubRunner(client)));
                pubsubThread.Start();
            }
        }

        private void pubsubRunner(ILocalHueClient client)
        {
            Console.WriteLine("Started pubsub");

            var clientsManager = new PooledRedisClientManager("huelightbot.com");
            var redisPubSub = new RedisPubSubServer(clientsManager, "channel-1", "channel-2")
            {
                OnMessage = (channel, msg) => HandleOnMessage(channel,msg)
            }.Start();


            return;
        }

        private void HandleOnMessage(string channel, string msg)
        {
            Console.WriteLine("Received '{0}' from '{1}'", msg, channel)
        }


        private void SetHexColor(string hex)
        {
            var command = new LightCommand();
            command.SetColor(new RGBColor(hex));

            client.SendCommandAsync(command);
        }

        private void SetLightsOff()
        {
            var command = new LightCommand();
            command.TurnOff();

            client.SendCommandAsync(command);
        }

        private void SetLightsOn()
        {
            var command = new LightCommand();
            command.TurnOn();

            client.SendCommandAsync(command);
        }
    }
}
