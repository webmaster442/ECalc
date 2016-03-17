using System.Reflection;
using System.Windows;

namespace ECalc
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            VersionString.Text = string.Format("Version: {0}", version);
        }
    }
}
