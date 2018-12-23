using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LuaSTGEditorSharp.Windows
{
    using Application = System.Windows.Application;
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window , INotifyPropertyChanged
    {
        App mainApp = Application.Current as App;

        static readonly int[] resX = { 640, 960, 1280 };
        static readonly int[] resY = { 480, 720, 960 };

        public string ZipExecutablePath
        {
            get => mainApp.ZipExecutablePath;
            set
            {
                mainApp.ZipExecutablePath = value;
                RaiseProertyChanged("ZipExecutablePath");
            }
        }

        public string LuaSTGExecutablePath
        {
            get => mainApp.LuaSTGExecutablePath;
            set
            {
                mainApp.LuaSTGExecutablePath = value;
                RaiseProertyChanged("LuaSTGExecutablePath");
            }
        }

        public string TempPath
        {
            get => mainApp.TempPath;
            set
            {
                mainApp.TempPath = value;
                RaiseProertyChanged("TempPath");
            }
        }

        public int DebugResolutionX
        {
            get => mainApp.DebugResolutionX;
            set
            {
                mainApp.DebugResolutionX = value;
                RaiseProertyChanged("DebugResolutionX");
            }
        }

        public int DebugResolutionY
        {
            get => mainApp.DebugResolutionY;
            set
            {
                mainApp.DebugResolutionY = value;
                RaiseProertyChanged("DebugResolutionY");
            }
        }

        public int IndexedReso
        {
            get
            {
                switch(DebugResolutionX)
                {
                    case 640:
                        return 0;
                    case 960:
                        return 1;
                    default:
                        return 2;
                }
            }
            set
            {
                DebugResolutionX = resX[value];
                DebugResolutionY = resY[value];
                RaiseProertyChanged("IndexedReso");
            }
        }

        public bool DebugWindowed
        {
            get => mainApp.DebugWindowed;
            set
            {
                mainApp.DebugWindowed = value;
                RaiseProertyChanged("DebugWindowed");
            }
        }

        public bool DebugCheat
        {
            get => mainApp.DebugCheat;
            set
            {
                mainApp.DebugCheat = value;
                RaiseProertyChanged("DebugCheat");
            }
        }

        public bool DebugUpdateLib
        {
            get => mainApp.DebugUpdateLib;
            set
            {
                mainApp.DebugUpdateLib = value;
                RaiseProertyChanged("DebugUpdateLib");
            }
        }

        public bool DebugSaveProj
        {
            get => mainApp.DebugSaveProj;
            set
            {
                mainApp.DebugSaveProj = value;
                RaiseProertyChanged("DebugSaveProj");
            }
        }

        public bool PackProj
        {
            get => mainApp.PackProj;
            set
            {
                mainApp.PackProj = value;
                RaiseProertyChanged("PackProj");
            }
        }

        public bool AutoMoveToNew
        {
            get => mainApp.AutoMoveToNew;
            set
            {
                mainApp.AutoMoveToNew = value;
                RaiseProertyChanged("AutoMoveToNew");
            }
        }

        public bool MD5Check
        {
            get => mainApp.SaveResMeta;
            set
            {
                mainApp.SaveResMeta = value;
                RaiseProertyChanged("MD5Check");
            }
        }

        public string AuthorName
        {
            get => mainApp.AuthorName;
            set
            {
                mainApp.AuthorName = value;
                RaiseProertyChanged("AuthorName");
            }
        }

        public SettingsWindow()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaiseProertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void ButtonZipExecutablePath_Click(object sender, RoutedEventArgs e)
        {
            var chooseExc = new OpenFileDialog()
            {
                Filter = "Zip Executable|7z.exe"
            };
            if (chooseExc.ShowDialog() != System.Windows.Forms.DialogResult.Cancel) 
            {
                ZipExecutablePath = chooseExc.FileName;
            }
        }

        private void ButtonLuaSTGExecutablePath_Click(object sender, RoutedEventArgs e)
        {
            var chooseExc = new OpenFileDialog()
            {
                Filter = "LuaSTG Executable|LuaSTGPlus.dev.exe"
            };
            if (chooseExc.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                LuaSTGExecutablePath = chooseExc.FileName;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
