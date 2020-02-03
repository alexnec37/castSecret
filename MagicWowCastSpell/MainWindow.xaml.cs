using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Gma.System.MouseKeyHook;
using Gma.System.MouseKeyHook.Implementation;

namespace MagicWowCastSpell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private IKeyboardMouseEvents m_Events;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // 80ff80 = 2
            // 8080ff = 3
            
            IntPtr appHandle = FindWindow(null, "World of Warcraft");

            var pix = new ColorOfPixel();
            Color cc = pix.getColor(1, 1, appHandle);
            listBox1.Items.Add(cc.ToString() + " - " + cc.R + ":" + cc.G + ":" + cc.B);
            
        }
        
        private void SubscribeGlobal()
        {
            Unsubscribe();
            Subscribe(Hook.GlobalEvents());
        }

        private void Subscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;
            //m_Events.KeyDown += OnKeyDown;
            //m_Events.KeyUp += OnKeyUp;
            //m_Events.KeyPress += HookManager_KeyPress;

            //m_Events.MouseUp += OnMouseUp;
            //m_Events.MouseClick += OnMouseClick;
            //m_Events.MouseDoubleClick += OnMouseDoubleClick;

            //m_Events.MouseMove += HookManager_MouseMove;

            //m_Events.MouseDragStarted += OnMouseDragStarted;
            //m_Events.MouseDragFinished += OnMouseDragFinished;

            //if (checkBoxSupressMouseWheel.Checked)
            //    m_Events.MouseWheelExt += HookManager_MouseWheelExt;
            //else
            //    m_Events.MouseWheel += HookManager_MouseWheel;

            //if (checkBoxSuppressMouse.Checked)
            //    m_Events.MouseDownExt += HookManager_Supress;
            //else
            //    m_Events.MouseDown += OnMouseDown;

        }

        private void Unsubscribe()
        {
            if (m_Events == null) return;

            //m_Events.MouseClick -= OnMouseClick;

            m_Events.Dispose();
            m_Events = null;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            //Log(string.Format("MouseClick \t\t {0}\n", e.Button));
        }
    }
}
