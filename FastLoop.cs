using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Syncr
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr hWnd;
        public Int32 msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }

    public class FastLoop
    {
        PreciseTimer _timer = new PreciseTimer();
        LoopCallback _callback;
        Thread updateThread;
        bool closeflag = false;
        public delegate void LoopCallback(double elapsed);
        public FastLoop(LoopCallback callback)
        {
            _callback = callback;
            //Application.Idle += new EventHandler(OnApplicationEnterIdle);
        }

        public void Start()
        {
            updateThread = new Thread(Update);
            updateThread.Start();
        }

        public void Close()
        {
            closeflag = true;
        }

        void Update()
        {
            while (!closeflag)
            {
                _callback(_timer.GetElapsedTime());
            }
        }

        private void OnApplicationEnterIdle(object sender, EventArgs e)
        {
            while (IsAppStillIdle())
            {
                _callback(_timer.GetElapsedTime());
            }
        }

        private bool IsAppStillIdle()
        {
            Message msg;
            return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msf, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
    }
}

