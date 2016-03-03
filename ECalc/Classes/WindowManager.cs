using System.Collections.Generic;
using System.Windows;

namespace ECalc.Classes
{
    /// <summary>
    /// Window management class
    /// </summary>
    internal static class WindowManager
    {
        private static List<Window> _childs;

        static WindowManager()
        {
            _childs = new List<Window>();
        }

        /// <summary>
        /// Brings a window to the front
        /// </summary>
        /// <param name="w">Window to bring to front</param>
        public static void BringToFront(Window w)
        {
            if (w.WindowState == WindowState.Minimized)
                w.WindowState = WindowState.Normal;
            w.Activate();
        }

        /// <summary>
        /// Begins tracking of a child window
        /// </summary>
        /// <param name="w">Window to track</param>
        public static void RegisterChild(Window w)
        {
            _childs.Add(w);
        }

        /// <summary>
        /// Stops tracking of a child window
        /// </summary>
        /// <param name="w"></param>
        public static void UnRegisterChild(Window w)
        {
            _childs.Remove(w);
        }

        /// <summary>
        /// Minimizes all tracked child windows
        /// </summary>
        public static void MinimizeAll()
        {
            foreach (var window in _childs)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        /// <summary>
        /// Restores all tracked child windows
        /// </summary>
        public static void RestoreAll()
        {
            foreach (var window in _childs)
            {
                window.WindowState = WindowState.Normal;
            }
        }
    }
}
