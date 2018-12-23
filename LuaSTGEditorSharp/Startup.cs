using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace LuaSTGEditorSharp
{
    public class Startup
    {
        [STAThread()]
        public static void Main(string[] args)
        {
            /*
            string arg = AppDomain.CurrentDomain.SetupInformation.ActivationArguments?.ActivationData?[0];
            if (args.Length < 0) 
            {
                args = new string[] { arg };
            }
            
            SingleInstanceWrapper singleInstanceWrapper = new SingleInstanceWrapper(new ReadOnlyCollection<string>(args));
            */
            SingleInstanceWrapper singleInstanceWrapper = new SingleInstanceWrapper();
            singleInstanceWrapper.Run(args);
        }
    }
}
