using ECalc.Controls;
using System.Windows;
using System.Windows.Controls;
using ECalc.Docs;
using System.Deployment.Application;
using System;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            var w = new AboutDialog();
            MainWindow.ShowDialog(w);
        }

        private void BtnIssue_Click(object sender, RoutedEventArgs e)
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "https://github.com/webmaster442/ECalc/issues";
            p.Start();
        }

        private void BtnScreenKeyboard_Click(object sender, RoutedEventArgs e)
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "osk.exe";
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        private void BtnDocs_Click(object sender, RoutedEventArgs e)
        {
            var dv = new DocumentationViewer();
            MainWindow.SwithToControl(dv);
        }

        private void BtnDonate_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=AVUW7CAHC2LES");
        }

        private void BtnWindManager_Click(object sender, RoutedEventArgs e)
        {
            var wm = new WindowsManager();
            MainWindow.SwithToControl(wm);
        }

        private void BtnChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            App.NextTheme();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\n"+
                        "Please check your network connection, or try again later.\n"+
                        "Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Cannot check for a new version of the application.\n"+
                        "The ClickOnce deployment is corrupt. Please redeploy the application and try again.\n"
                        +"Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application.\n"+
                        "Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        var dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButton.YesNoCancel);
                        if (dr == MessageBoxResult.No) doUpdate = false;
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application. \n\n"+
                                "Please check your network connection, or try again later. \n"+
                                "Error: " + dde);
                            return;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application.");
            }
        }
    }
}
