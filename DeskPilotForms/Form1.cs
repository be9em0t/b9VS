using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PInvoke;
using System.Diagnostics;
using DeskPilotForms2;

// ToDo: list all running programs
// ToDo: parse connected monitors and asign programs on startup ar on user command
// ToDo: create reasonable UI
// Done: create tray icon

// ToDo: Functionality
//      - ability to move any offscreen window inside of monitor frame
//      - Control on which monitor program windows appear (main window, child windows)
//      - Do something when a program window appears (move window depending on conditions, min/hide, move to screen, send message)
//      - Do something if program is not running
//      - Control for conditions (number of screens, running programs)

// ToDo: UI Workflow
//      -    
//

namespace DeskPilotForms
{

    public partial class Form1 : Form
    {

        List<IntPtr> topWinHandleList = new List<IntPtr>();

        ArrayList topWinHandles = new ArrayList();
        ArrayList childWinHandles = new ArrayList();


        public Form1()
        {
            InitializeComponent();
            notifyIcon1.Visible = false;
        }

        public IntPtr findWin()
        {
            // Get window handle
            IntPtr winHandle = PInvoke.User32.FindWindow("Notepad", "Untitled - Notepad");
            return winHandle;
        }

        // button test notepad
        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr winHandle = findWin();

            // Position and active window
            PInvoke.User32.MoveWindow(winHandle, 10, 10, 500, 500, true);
            PInvoke.User32.ShowWindow(winHandle,User32.WindowShowStyle.SW_SHOWNORMAL);
            PInvoke.User32.SetForegroundWindow(winHandle);
        }

        // button list all porcesses
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            listBox1.BeginUpdate();

            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    //Console.WriteLine("thread count: " + process.Threads.Count);
                    topWinHandles.Add(process.MainWindowHandle);
                    listBox1.Items.Add(process.ProcessName);
                    Console.WriteLine(process.ProcessName);
                    List<IntPtr> childHands = WindowEnumTest.GetChildWindows(process.MainWindowHandle);
                    Console.WriteLine(childHands.Count);
                    foreach (IntPtr chHand in childHands)
                    {
                        Console.WriteLine( PinvokeMethods.GetText(chHand));
                    }

                    //IntPtr hand = process.MainWindowHandle; // PInvoke.User32.FindWindow("XYZ_Widget_1", null);
                    //if (hand != null)
                    //{
                    //    hand = PInvoke.User32.FindWindowEx(hand, IntPtr.Zero, null, null);
                    //    if (hand != null)
                    //    {
                    //        //Console.WriteLine("child: " + hand + " : " + PInvoke.User32.IsWindow(hand));
                    //        if (PInvoke.User32.IsWindow(hand) == true)
                    //        {
                    //            Console.WriteLine("child: " + hand + " : ");
                    //        }
                    //        //hand = PInvoke.User32.FindWindowEx(hand, IntPtr.Zero, "XYZ_Renderer", null);
                    //        // and so on... 
                    //    }
                    //}


                    //foreach (ProcessThread thread in process.Threads)
                    //    PInvoke.User32.EnumThreadWindows(thread.Id,
                    //        (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

                    //return handles;

                }
            }
            listBox1.EndUpdate();
            //Console.WriteLine(topWindows.Count);

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Debug.WriteLine(listBox1.SelectedItem);
            //try
            //{
            //    IntPtr childHand = PInvoke.User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, null);
            //    Debug.WriteLine(childHand);
            //}
            //catch
            //{
            //    Debug.WriteLine("error");
            //}
        }

        private void listBox1_Click(object sender, EventArgs e)
        {

            listBox2.Items.Clear();
            listBox2.BeginUpdate();


            //IntPtr tmp = (IntPtr) topWinHandles[listBox1.SelectedIndex];
            //IntPtr childHand = PInvoke.User32.FindWindowEx(tmp, IntPtr.Zero, null, null);

            //var handles = new List<IntPtr>();

            //listBox2.Items.Add(childHand);

            //try
            //{
            //    IntPtr childHand = PInvoke.User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, null);
            //    //Debug.WriteLine(childHand);
            //    listBox2.Items.Add(listBox1.SelectedIndex);
            //}
            //catch
            //{
            //    Debug.WriteLine("error");
            //}

            listBox2.EndUpdate();
        }

        // Traymin related
        private void Form1_Resize(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "MinToTray";
            notifyIcon1.BalloonTipText = "it works!";

            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }

        }

        // Tray icon related
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            PinvokeMethods.EnumWindowsMain();
            //ConsumeChildWins.MainMethAll();
        }
    }
}
