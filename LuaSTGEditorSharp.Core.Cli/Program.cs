using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.Plugin.Default;
using Newtonsoft.Json;
using static System.Console;

namespace LuaSTGEditorSharp.Core.Cli
{
    internal class Program
    {
        static AppSetting Setting = new();
        static string PluginPath = "LuaSTGSubLib.dll";
        static string OutputDirectory = "";
        static string OutputFilename = "";

        static readonly DocumentCollection Documents = new();
#pragma warning disable CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
        static DocumentData CurrentDocument = null;
#pragma warning restore CS8625 // 无法将 null 字面量转换为非 null 的引用类型。

        static void Main(string[] args)
        {
            if (SolveArgs(args))
            {
                Init();
                if (args.Length > 0)
                {
                    OpenFile(args[0]);
                    PackProject();
                    WriteLine("Finish");
                }
            }
        }

        static bool SolveArgs(string[] args)
        {
            ArgsResolver ar;
            try
            {
                ar = new(new(args));
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message + " type -h for more info.");
                return false;
            }
            bool flag = false;
            bool help = ar.IsHelp;
            if (ar.File != null)
            {
                flag = true;
            }
            else
            {
                help = true;
            }
            if (help)
            {
                WriteLine("LuaSTGEditorSharp.Core.Cli [path][-o path][-p path][-h]");
                WriteLine();
                WriteLine("\tpath\t\tPath of the source .lstges or .lstgproj");
                WriteLine();
                WriteLine("\t-d\tdir\tIf given, set the output directory.");
                WriteLine("\t\t\tDefault output directory is '{path of this executable}\\mod\\'.");
                WriteLine();
                WriteLine("\t-n\tname\tIf given, set the output filename.");
                WriteLine("\t\t\tDefault output filename is '{predefined name by project file}.zip'.");
                WriteLine();
                WriteLine("\t-p\tpath\tIf given, set the plugin for compilation.");
                WriteLine("\t\t\tDefault is 'LuaSTGSubLib.dll'.");
                WriteLine();
                WriteLine("\t-h\t\tShow help.");
                WriteLine();
                return false;
            }
            if (ar.OutputDirectory != null)
            {
                OutputDirectory = ar.OutputDirectory;
            }
            if (ar.OutputFilename != null)
            {
                OutputFilename = ar.OutputFilename;
            }
            if (ar.Plugin != null)
            {
                PluginPath = ar.Plugin;
            }
            return flag;
        }

        static void Init()
        {
            Setting = new AppSetting();
            try
            {
                WriteLine($"Loading Plugin : {PluginPath}");
                PluginHandler.DefaultPlugin = new DefaultPluginEntry();
                Exception loadplugExc = PluginHandler.LoadPlugin(PluginPath);
                if (loadplugExc != null)
                {
                    WriteLine($"Load Plugin Failed.\n{loadplugExc}");
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString());
            }
            Lua.IndentationGenerator.Current = new Lua.SpaceIndentation() { NumOfSpaces = 4 };
        }

        static void OpenFile(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Uri fileUri = new Uri(path);
                string fp = Uri.UnescapeDataString(fileUri.AbsolutePath);
                OpenDocFromPath(Path.GetFileName(fp), fp);
            }
        }

        static void OpenDocFromPath(string name, string path)
        {
            try
            {
                CurrentDocument = DocumentData.GetNewByExtension(Path.GetExtension(path), Documents.MaxHash, name, path);
                Documents.AddAndAllocHash(CurrentDocument);
                TreeNodeBase t = CurrentDocument.CreateNodeFromFile(path);
                CurrentDocument.TreeNodes.Add(t);
                t.RaiseCreate(new OnCreateEventArgs() { parent = null });
                CurrentDocument.OnOpening();
            }
            catch (JsonException e)
            {
                WriteLine("Failed to open document. Please check whether the targeted file is in current version.\n"
                    + e.ToString());
            }
        }

        static void PackProject()
        {
            string modFolder = string.IsNullOrWhiteSpace(OutputDirectory)
                ? Path.GetDirectoryName(Setting.LuaSTGExecutablePath) + "\\mod\\"
                : OutputDirectory;
            if (!Directory.Exists(modFolder))
            {
                Directory.CreateDirectory(modFolder);
            }
            DocumentData doc;
            if (!(CurrentDocument is PlainDocumentData pdd && pdd.parentProj != null))
            {
                doc = CurrentDocument;
            }
            else
            {
                doc = pdd.parentProj;
            }
            doc.GatherCompileInfo(Setting, OutputDirectory, OutputFilename);
            doc.CompileProcess.ProgressChanged += (s, e) => WriteLine($"[{e.ProgressPercentage}] {e.UserState}");
            doc.CompileProcess.ExecuteProcess(false, false, Setting, OutputDirectory, OutputFilename);
        }

        public class AppSetting : IAppSettings
        {
            public string ZipExecutablePath { get; } = string.Empty;
            public string LuaSTGExecutablePath { get; } = System.Windows.Forms.Application.ExecutablePath;
            public bool IsEXEPathSet { get; } = true;
            public string TempPath { get; } = Path.GetTempPath();
            public string SLDir { get; set; } = string.Empty;
            public bool SaveResMeta { get; } = false;
            public bool PackProj { get; } = true;
            public string PackerType { get; } = "zip-internal-quick";
            public bool SpaceIndentation { get; } = true;
            public int IndentationSpaceLength { get; } = 0;
        }
    }
}