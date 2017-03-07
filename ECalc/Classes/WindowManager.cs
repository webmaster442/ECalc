using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using AppLib.WPF;
using AppLib.WPF.Extensions;

namespace ECalc.Classes
{

    internal enum WindowSizes
    {
        Large, Normal
    }

    /// <summary>
    /// Window management class
    /// </summary>
    internal static class WindowManager
    {
        private readonly static List<Window> _childs;

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
        /// Closes all child windows
        /// </summary>
        /// <returns>true, if all child windows has been closed</returns>
        public static bool CloseAll()
        {
            if (_childs.Count > 0)
            {
                var question = MessageBox.Show("Close all open windows ?", "Close Windows", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (question == MessageBoxResult.Yes)
                {
                    while (_childs.Count > 0)
                    {
                        var child = _childs[0];
                        _childs.RemoveAt(0);
                        child.Close();
                        child = null;
                    }
                    return true;
                }
                return false;
            }
            return true;
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

        /// <summary>
        /// Returns a window based on it's index in the _childs collection
        /// </summary>
        /// <param name="index">index of the window</param>
        /// <returns>a Window instance</returns>
        public static Window GetWindow(int index)
        {
            return _childs[index];
        }

        public static void ResizeWindows(WindowSizes ws)
        {
            var screen = ScreenHelper.GetCurrentScreenSize(Application.Current.MainWindow);
            double w, h;
            if (ws == WindowSizes.Normal)
            {
                w = 960;
                h = 540;
            }
            else
            {
                w = 1152;
                h = 648;
            }

            if (screen.Width < w || screen.Height < h)
            {
                MainWindow.ErrorDialog("Current monitor does not have the needed resolution for large mode");
                return;
            }

            Application.Current.MainWindow.Width = w;
            Application.Current.MainWindow.Height = h;
            foreach (var window in _childs)
            {
                window.Width = w;
                window.Height = h;
            }
        }

        /// <summary>
        /// Renders the previews for each child window 
        /// </summary>
        public static ObservableCollection<WindowData> Previews
        {
            get
            {
                var ret = new ObservableCollection<WindowData>();

                foreach (var window in _childs)
                {
                    ret.Add(new WindowData
                    {
                        Image = window.Render(),
                        Title = window.Title
                    });
                }

                return ret;
            }
        }
    }

    internal class WindowData
    {
        public string Title { get; set; }
        public ImageSource Image { get; set; }
    }
}
