using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace Syncr
{
    public struct Window
    {
        public IntPtr handle;
        public string name;
        public string group;
        public Window(IntPtr w, string n, string g = "Misc")
        {
            handle = w;
            name = n;
            group = g;
        }
    }

    public class Win32
    {
        const int MAXTITLE = 255;

        private static List<Window> lstTitles;

        private delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        #region DLL
        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EnumDesktopWindows(IntPtr hDesktop,
        EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int _GetWindowText(IntPtr hWnd,
        StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        static readonly IntPtr HWND_TOP = new IntPtr(-1);
        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOACTIVATE = 0x0010;

        #endregion

        public const UInt32 WM_CLOSE = 0x0010;

        private static bool EnumWindowsProc(IntPtr hWnd, int lParam)
        {
            string strTitle = GetWindowText(hWnd);
            if (strTitle != "" & IsWindowVisible(hWnd)) //
            {
                lstTitles.Add(new Window(hWnd, strTitle));
            }
            return true;
        }

        /// <summary>
        /// Return the window title of handle
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static string GetWindowText(IntPtr hWnd)
        {
            StringBuilder strbTitle = new StringBuilder(MAXTITLE);
            int nLength = _GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
            strbTitle.Length = nLength;
            return strbTitle.ToString();
        }

        /// <summary>
        /// Return titles of all visible windows on desktop
        /// </summary>
        /// <returns>List of titles in type of string</returns>
        public static Window[] GetDesktopWindowsTitles()
        {
            lstTitles = new List<Window>();
            EnumDelegate delEnumfunc = new EnumDelegate(EnumWindowsProc);
            bool bSuccessful = EnumDesktopWindows(IntPtr.Zero, delEnumfunc, IntPtr.Zero); //for current desktop

            if (bSuccessful)
            {
                return lstTitles.ToArray();
            }
            else
            {
                // Get the last Win32 error code
                int nErrorCode = Marshal.GetLastWin32Error();
                string strErrMsg = String.Format("EnumDesktopWindows failed with code {0}.", nErrorCode);
                throw new Exception(strErrMsg);
            }
        }

        ///<summary> 
        ///Close the specific window
        ///</summary>
        public static void CloseWindow(IntPtr handle)
        {
            if (handle != IntPtr.Zero)
                Win32.SendMessage(handle, Win32.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        public static void SetWindowTopMost(IntPtr handle)
        {
            SetWindowPos(handle, HWND_TOP, 0, 0, 0, 0, 0x002 | 0x0001);
        }

        public static void SetWindowBottom(IntPtr handle)
        {
            SetWindowPos(handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
        }
    }
}