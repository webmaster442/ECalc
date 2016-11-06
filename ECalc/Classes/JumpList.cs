using System.Reflection;
using System.Windows;
using System.Windows.Shell;

namespace ECalc.Classes
{
    internal static class AppJumpList
    {
        public static void CreateJumpList()
        {
            JumpTask update = new JumpTask()
            {
                Title = "Check for Updates",
                Arguments = "/update",
                Description = "Cheks for Software Updates",
                CustomCategory = "Actions",
                IconResourcePath = Assembly.GetEntryAssembly().CodeBase,
                ApplicationPath = Assembly.GetEntryAssembly().CodeBase
            };
            JumpTask restart = new JumpTask()
            {
                Title = "Restart on default monitor",
                Arguments = "/firstmonitor",
                Description = "Restarts the application on the first monitor",
                CustomCategory = "Actions",
                IconResourcePath = Assembly.GetEntryAssembly().CodeBase,
                ApplicationPath = Assembly.GetEntryAssembly().CodeBase
            };

            var appmenu = new JumpList();
            appmenu.JumpItems.Add(update);
            appmenu.JumpItems.Add(restart);
            appmenu.ShowFrequentCategory = false;
            appmenu.ShowRecentCategory = false;

            JumpList.SetJumpList(Application.Current, appmenu);
        }
    }
}
