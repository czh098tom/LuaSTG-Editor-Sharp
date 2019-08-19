using System;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.Properties;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application, IMessageThrowable
    {
        public static int mxUAttr = 100;

        public App()
        {
            PropertyChanged += new PropertyChangedEventHandler(CheckMessage);
        }

        public void InitDict()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            TextWriter tw = Console.Out;
            try
            {
                string tempPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "LuaSTG Editor/"));
                if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
                fs = new FileStream(Path.GetFullPath(Path.Combine(tempPath, "log.txt"))
                    , FileMode.OpenOrCreate, FileAccess.Write);
                sw = new StreamWriter(fs);
                Console.SetOut(sw);
                base.OnStartup(e);
                RaiseProertyChanged("m");
                if (!PluginHandler.LoadPlugin(PluginPath))
                {
                    MessageBox.Show("Load Plugin Failed.");
                }

                Lua.SyntaxHighlightLoader.LoadLuaDef();

                var mainWindow = new MainWindow();
                //string arg = AppDomain.CurrentDomain.SetupInformation.ActivationArguments?.ActivationData?[0];
                string arg = "";
                if (e.Args.Length > 0)
                {
                    arg = e.Args[0];
                }
                if (!string.IsNullOrEmpty(arg))
                {
                    Uri fileUri = new Uri(arg);
                    string fp = Uri.UnescapeDataString(fileUri.AbsolutePath);
                    //MessageBox.Show(fp);
                    LoadDoc(fp);
                    //LoadDoc(arg);
                }
                MainWindow = mainWindow;
                MainWindow.Show();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Current.Shutdown();
            }
            finally
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
        }

        public ObservableCollection<MessageBase> Messages { get; } = new ObservableCollection<MessageBase>();

        public IInputWindowSelector InputWindowSelector { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseProertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            if (!File.Exists(LuaSTGExecutablePath) || Path.GetFileName(LuaSTGExecutablePath) != "LuaSTGPlus.dev.exe")
            {
                a.Add(new EXEPathNotSetMessage(LuaSTGExecutablePath, "LuaSTG Path", 0, this));
            }
            if (BatchPacking && (!File.Exists(ZipExecutablePath) || Path.GetFileName(ZipExecutablePath) != "7z.exe"))
            {
                a.Add(new EXEPathNotSetMessage(ZipExecutablePath, "7z Path", 0, this));
            }
            return a;
        }

        public void CheckMessage(object sender, PropertyChangedEventArgs e)
        {
            var a = GetMessage();
            Messages.Clear();
            foreach (MessageBase mb in a)
            {
                Messages.Add(mb);
            }
            MessageContainer.UpdateMessage(this);
        }

        public void LoadDoc(string arg)
        {
            (MainWindow as MainWindow).OpenDocFromPath(Path.GetFileName(arg), arg);
        }

        public string ZipExecutablePath
        {
            get => Settings.Default.ZipExecutablePath;
            set
            {
                Settings.Default["ZipExecutablePath"] = value;
                RaiseProertyChanged("ZipExecutablePath");
            }
        }

        public string LuaSTGExecutablePath
        {
            get => Settings.Default.LuaSTGExecuteablePath;
            set
            {
                Settings.Default["LuaSTGExecuteablePath"] = value;
                RaiseProertyChanged("LuaSTGExecuteablePath");
            }
        }

        public int DebugResolutionX
        {
            get => Settings.Default.DebugResolutionX;
            set
            {
                Settings.Default["DebugResolutionX"] = value;
            }
        }

        public int DebugResolutionY
        {
            get => Settings.Default.DebugResolutionY;
            set
            {
                Settings.Default["DebugResolutionY"] = value;
            }
        }

        public bool DebugWindowed
        {
            get => Settings.Default.DebugWindowed;
            set
            {
                Settings.Default["DebugWindowed"] = value;
            }
        }
        
        public bool DebugCheat
        {
            get => Settings.Default.DebugCheat;
            set
            {
                Settings.Default["DebugCheat"] = value;
            }
        }

        public bool DebugUpdateLib
        {
            //get => Settings.Default.DebugUpdateLib;
            get => false;
            set
            {
                Settings.Default["DebugUpdateLib"] = value;
            }
        }

        public bool DebugSaveProj
        {
            get => Settings.Default.DebugSaveProj;
            set
            {
                Settings.Default["DebugSaveProj"] = value;
            }
        }

        public bool PackProj
        {
            get => Settings.Default.PackProj;
            set
            {
                Settings.Default["PackProj"] = value;
            }
        }

        public bool SaveResMeta
        {
            get => Settings.Default.SaveResMeta;
            set
            {
                Settings.Default["SaveResMeta"] = value;
            }
        }

        public bool AutoMoveToNew
        {
            get => Settings.Default.AutoMoveToNew;
            set
            {
                Settings.Default["AutoMoveToNew"] = value;
            }
        }

        public string AuthorName
        {
            get => Settings.Default.AuthorName;
            set
            {
                Settings.Default["AuthorName"] = value;
            }
        }

        public string PluginPath
        {
            get => Settings.Default.LuaSTGNodeLibPath;
            set
            {
                Settings.Default["LuaSTGNodeLibPath"] = value;
            }
        }

        public string TempPath
        {
            get => Settings.Default.TempPath;
            set
            {
                Settings.Default["TempPath"] = value;
            }
        }

        public bool BatchPacking
        {
            get => !Settings.Default.UseInternalZipCompressor;
            set
            {
                Settings.Default["UseInternalZipCompressor"] = !value;
            }
        }

        public string SLDir
        {
            get => Settings.Default.SLDir;
            set
            {
                Settings.Default["SLDir"] = value;
            }
        }

        public bool IsEXEPathSet
        {
            get=> !(BatchPacking && string.IsNullOrEmpty(ZipExecutablePath)) || string.IsNullOrEmpty(LuaSTGExecutablePath);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
            Current.Shutdown();
        }
    }
}
