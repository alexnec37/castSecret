using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MagicWowCastSpell;
using System.Windows.Media;
using Gma.System.MouseKeyHook;
using System.Threading;


namespace WowMagicCast
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents m_Events;
        private IntPtr appHandle;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", SetLastError = true)]
        static extern void mouse_event(MouseFlags dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

        [Flags]
        enum MouseFlags
        {
            Move = 0x0001, LeftDown = 0x0002, LeftUp = 0x0004, RightDown = 0x0008,
            RightUp = 0x0010, Absolute = 0x8000
        };

        public Form1()
        {
            InitializeComponent();
            //SubscribeGlobal();
            FormClosing += Main_Closing;
            appHandle = FindWindow(null, "World of Warcraft");
        }

        private void Main_Closing(object sender, CancelEventArgs e)
        {
            //Unsubscribe();
            Unsubscribe();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SubscribeGlobalMouse();
        }

        private void SubscribeGlobalMouse()
        {
            Unsubscribe();
            Subscribe(Hook.GlobalEvents());
        }

        private void Subscribe(IKeyboardMouseEvents events)
        {
            Unsubscribe();
            m_Events = events;
            m_Events.KeyDown += OnCtrlClick;
            listBox1.Items.Add("Subscribe - " + DateTime.Now);
            //m_Events.MouseClick += OnMouseClick;
        }

        private void Unsubscribe()
        {
            if (m_Events == null) return;
            
            m_Events.KeyDown -= OnCtrlClick;
            //m_Events.MouseClick -= OnMouseClick;

            m_Events.Dispose();
            m_Events = null;
            listBox1.Items.Add("Unsubscribe - " + DateTime.Now);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            // ff8080 = 1
            // 80ff80 = 2
            // 8080FF = 3
            // FFFF80 = 4
            // 80FFFF = 54
            // FF80FF = 6

            //mouse_event(MouseFlags.LeftDown, 600, 320, 0, appHandle);
            //mouse_event(MouseFlags.LeftUp, 600, 320, 0, appHandle);

            Thread.Sleep(100);

            var pix = new ColorOfPixel();
            Color cc = pix.getColor(1, 1, appHandle);

            //listBox1.Items.Add(cc.ToString() + " - " + cc.R + ":" + cc.G + ":" + cc.B);

            //listBox1.Items.Add("press");

            switch (cc.ToString())
            {
                case "#01FF8080":
                    SendKeys.SendWait("1");
                    break;
                case "#0180FF80":
                    SendKeys.SendWait("2");
                    break;
                case "#018080FF":
                    SendKeys.SendWait("3");
                    break;
                case "#01FFFF80":
                    SendKeys.SendWait("4");
                    break;
                case "#0180FFFF":
                    SendKeys.SendWait("5");
                    break;
                case "#01FF80FF":
                    SendKeys.SendWait("6");
                    break;
            }

            //textBoxLog.AppendText("text\n");
            //textBoxLog.ScrollToCaret();
        }


        private void OnCtrlClick(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.T) return;

            // ff8080 = 1
            // 80ff80 = 2
            // 8080FF = 3
            // FFFF80 = 4
            // 80FFFF = 54
            // FF80FF = 6


            //mouse_event(MouseFlags.LeftDown, 600, 320, 0, appHandle);
            //mouse_event(MouseFlags.LeftUp, 600, 320, 0, appHandle);

            //Thread.Sleep(100);

            var pix = new ColorOfPixel();
            Color cc = pix.getColor(1, 1, appHandle);

            //listBox1.Items.Add(cc.ToString() + " - " + cc.R + ":" + cc.G + ":" + cc.B);

            //listBox1.Items.Add("press");

            switch (cc.ToString())
            {
                case "#01FF8080":
                    SendKeys.SendWait("1");
                    break;
                case "#0180FF80":
                    SendKeys.SendWait("2");
                    break;
                case "#018080FF":
                    SendKeys.SendWait("3");
                    break;
                case "#01FFFF80":
                    SendKeys.SendWait("4");
                    break;
                case "#0180FFFF":
                    SendKeys.SendWait("5");
                    break;
                case "#01FF80FF":
                    SendKeys.SendWait("6");
                    break;
                case "#01B280FF":
                    SendKeys.SendWait("7");
                    break;
                case "#0180B2FF":
                    SendKeys.SendWait("8");
                    break;
            }

            //Thread.Sleep(200);

            //mouse_event(MouseFlags.LeftDown, 600, 320, 0, appHandle);
            //mouse_event(MouseFlags.LeftUp, 600, 320, 0, appHandle);

            //textBoxLog.AppendText("text\n");
            //textBoxLog.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Unsubscribe();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
