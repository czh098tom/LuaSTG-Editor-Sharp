using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;
using LuaSTGEditorSharp.EditorData.Node;

using StartupEventArgs = Microsoft.VisualBasic.ApplicationServices.StartupEventArgs;

namespace LuaSTGEditorSharp
{
    public class SingleInstanceWrapper : WindowsFormsApplicationBase
    {
        public SingleInstanceWrapper()
        {
            IsSingleInstance = true;
        }

        public SingleInstanceWrapper(ReadOnlyCollection<string> s) : this()
        {
            this.InternalCommandLine = s;
        }

        private App app;

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            SplashScreen splashScreen = new SplashScreen("splashscreen.png");
            splashScreen.Show(true);
            app = new App();
            app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            string arg = null;
            if (eventArgs.CommandLine.Count > 0)
            {
                arg = eventArgs.CommandLine[0];
            }
            if (!string.IsNullOrEmpty(arg))
            {
                Uri fileUri = new Uri(arg);
                string fp = Uri.UnescapeDataString(fileUri.AbsolutePath);
                app.LoadDoc(fp);
            }
        }
    }
}
