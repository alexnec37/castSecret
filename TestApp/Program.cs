﻿namespace TestApp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Enumerate top-level and child windows
    /// </summary>
    /// <example>
    /// WindowsEnumerator enumerator = new WindowsEnumerator(); 
    /// foreach (ApiWindow top in enumerator.GetTopLevelWindows()) 
    /// { 
    ///    Console.WriteLine(top.MainWindowTitle); 
    ///        foreach (ApiWindow child in enumerator.GetChildWindows(top.hWnd))  
    ///            Console.WriteLine(" " + child.MainWindowTitle); 
    /// } 
    /// </example>
    public class WindowsEnumerator
    {

        private delegate int EnumCallBackDelegate(int hWnd, int lParam);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int EnumWindows(EnumCallBackDelegate lpEnumFunc, int lParam);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int EnumChildWindows(int hWndParent, EnumCallBackDelegate lpEnumFunc, int lParam);

        [DllImport("user32", EntryPoint = "GetClassNameA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int GetClassName(int hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int IsWindowVisible(int hWnd);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int GetParent(int hWnd);

        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern Int32 SendMessage(Int32 hWnd, Int32 wMsg, Int32 wParam, Int32 lParam);

        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern Int32 SendMessage(Int32 hWnd, Int32 wMsg, Int32 wParam, StringBuilder lParam);

        // my try to get all controls
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        //--------------------------


        // Top-level windows.
        // Child windows.
        // Get the window class.
        // Test if the window is visible--only get visible ones.
        // Test if the window's parent--only get the one's without parents.
        // Get window text length signature.
        // Get window text signature.

        private List<ApiWindow> _listChildren = new List<ApiWindow>();
        private List<ApiWindow> _listTopLevel = new List<ApiWindow>();

        private string _topLevelClass = "";
        private string _childClass = "";

        /// <summary>
        /// Get all top-level window information
        /// </summary>
        /// <returns>List of window information objects</returns>
        public List<ApiWindow> GetTopLevelWindows()
        {
            EnumWindows(EnumWindowProc, 0);

            return _listTopLevel;

        }

        public List<ApiWindow> GetTopLevelWindows(string className)
        {
            _topLevelClass = className;

            return this.GetTopLevelWindows();
        }

        /// <summary>
        /// Get all child windows for the specific windows handle (hwnd).
        /// </summary>
        /// <returns>List of child windows for parent window</returns>
        public List<ApiWindow> GetChildWindows(Int32 hwnd)
        {

            // Clear the window list.
            _listChildren = new List<ApiWindow>();

            // Start the enumeration process.
            EnumChildWindows(hwnd, EnumChildWindowProc, 0);

            // Return the children list when the process is completed.
            return _listChildren;
        }

        public List<ApiWindow> GetChildWindows(Int32 hwnd, string childClass)
        {

            // Set the search
            _childClass = childClass;

            return this.GetChildWindows(hwnd);

        }

        public static IList GetAllChildrenWindowHandles(IntPtr hParent, int maxCount)
        {
            IList result = new ArrayList();
            int ct = 0;
            IntPtr prevChild = IntPtr.Zero;
            IntPtr currChild = IntPtr.Zero;
            while (true && ct < maxCount)
            {
                currChild = FindWindowEx(hParent, prevChild, null, null);
                if (currChild == IntPtr.Zero) break;
                result.Add(currChild);
                prevChild = currChild;
                ++ct;
            }

            //IntPtr intPtr = new IntPtr(7415142);
            //int len = 200;// GetWindowTextLength(hWnd) + 1;
            //StringBuilder sb = new StringBuilder(len);
            //len = GetWindowText(intPtr, sb, len);
            //_listChildren.Add(new ApiWindow()
            //{
            //    MainWindowTitle = "get text = " + sb.ToString(0, len)
            //});

            return result;
        }

        /// <summary>
        /// Callback function that does the work of enumerating top-level windows.
        /// </summary>
        /// <param name="hwnd">Discovered Window handle</param>
        /// <returns>1=keep going, 0=stop</returns>
        private Int32 EnumWindowProc(Int32 hwnd, Int32 lParam)
        {

            // Eliminate windows that are not top-level.
            if (GetParent(hwnd) == 0 && Convert.ToBoolean(IsWindowVisible(hwnd)))
            {

                // Get the window title / class name.
                ApiWindow window = GetWindowIdentification(hwnd);

                 //Match the class name if searching for a specific window class.
                if (_topLevelClass.Length == 0 || window.ClassName.ToLower() == _topLevelClass.ToLower())
                {
                    _listTopLevel.Add(window);
                }
            }

            // To continue enumeration, return True (1), and to stop enumeration 
            // return False (0).
            // When 1 is returned, enumeration continues until there are no 
            // more windows left.

            return 1;

        }

        /// <summary>
        /// Callback function that does the work of enumerating child windows.
        /// </summary>
        /// <param name="hwnd">Discovered Window handle</param>
        /// <returns>1=keep going, 0=stop</returns>
        private Int32 EnumChildWindowProc(Int32 hwnd, Int32 lParam)
        {

            ApiWindow window = GetWindowIdentification(hwnd);

            // Attempt to match the child class, if one was specified, otherwise
            // enumerate all the child windows.
            //if (_childClass.Length == 0 || window.ClassName.ToLower() == _childClass.ToLower())
            //{
            //    _listChildren.Add(window);
            //}

            IntPtr intPtr = new IntPtr(7415142);
            int len = 200;// GetWindowTextLength(hWnd) + 1;
            StringBuilder sb = new StringBuilder(len);
            len = GetWindowText(intPtr, sb, len);
            _listChildren.Add(new ApiWindow()
            {
                MainWindowTitle = "get text = " + sb.ToString(0, len)
            });

            return 1;

        }

        /// <summary>
        /// Build the ApiWindow object to hold information about the Window object.
        /// </summary>
        private ApiWindow GetWindowIdentification(int hwnd)
        {

            const Int32 WM_GETTEXT = 13;
            const Int32 WM_GETTEXTLENGTH = 14;

            ApiWindow window = new ApiWindow();

            StringBuilder title = new StringBuilder();

            // Get the size of the string required to hold the window title.
            Int32 size = SendMessage(hwnd, WM_GETTEXTLENGTH, 0, 0);

            // If the return is 0, there is no title.
            if (size > 0)
            {
                title = new StringBuilder(size + 1);

                SendMessage(hwnd, WM_GETTEXT, title.Capacity, title);
            }

            // Get the class name for the window.
            StringBuilder classBuilder = new StringBuilder(64);
            GetClassName(hwnd, classBuilder, 64);

            // Set the properties for the ApiWindow object.
            window.ClassName = classBuilder.ToString();
            window.MainWindowTitle = title.ToString();
            window.hWnd = hwnd;

            return window;

        }

    }

    public class ApiWindow
    {
        public string MainWindowTitle = "";
        public string ClassName = "";
        public int hWnd;
    }


    class Program
    {
        static void Main(string[] args)
        {
            WindowsEnumerator enumerator = new WindowsEnumerator();
            foreach (ApiWindow top in enumerator.GetTopLevelWindows())
            {
                Console.WriteLine(top.MainWindowTitle + " hwnd = " + top.hWnd);
                foreach (ApiWindow child in enumerator.GetChildWindows(top.hWnd))
                    Console.WriteLine("  --- " + child.MainWindowTitle);
                Console.WriteLine("-------------------------------------------------");
            }

            //foreach (ApiWindow child in enumerator.GetChildWindows(2098574))//top.hWnd))
            //    Console.WriteLine(" " + child.MainWindowTitle);


            // launch app first
            //IntPtr appHandle = WindowsEnumerator.FindWindow(null, "World of Warcraft"); // P/Invoke
            //Console.WriteLine("App handle = " + appHandle.ToInt32());
            //IList children = WindowsEnumerator.GetAllChildrenWindowHandles(appHandle, 100);

            //Console.WriteLine("Children handles are:");
            //for (int i = 0; i < children.Count; ++i)
            //    Console.WriteLine(((IntPtr)children[i]).ToString("X"));

            Console.WriteLine("End");
            Console.ReadKey();
        }
    }
}
