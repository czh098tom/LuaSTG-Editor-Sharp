using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Compile;
using LuaSTGEditorSharp.EditorData.Exception;
using LuaSTGEditorSharp.Packer;

namespace LuaSTGEditorSharp.EditorData.Compile
{
    /// <summary>
    /// The <see cref="CompileProcess"/> of luastg project file (.lstgproj).
    /// </summary>
    internal class ProjectProcess : CompileProcess
    {
        public override string TargetPath => luaSTGFolder + "\\mod\\" + projName;

        /// <summary>
        /// The child <see cref="PartialProjectProcess"/> of the process need to do.
        /// </summary>
        internal readonly List<PartialProjectProcess> fileProcess = new List<PartialProjectProcess>();

        /// <summary>
        /// Execute the <see cref="CompileProcess"/>.
        /// </summary>
        /// <param name="SCDebug">Whether SCDebug is switched on.</param>
        /// <param name="StageDebug">Whether Stage Debug is switched on.</param>
        /// <param name="appSettings">App that contains settings</param>
        public override void ExecuteProcess(bool SCDebug, bool StageDebug, IAppSettings appSettings)
            => ExecuteProcess(SCDebug, StageDebug, appSettings, "", "");

        /// <summary>
        /// Execute the <see cref="CompileProcess"/>.
        /// </summary>
        /// <param name="SCDebug">Whether SCDebug is switched on.</param>
        /// <param name="StageDebug">Whether Stage Debug is switched on.</param>
        /// <param name="appSettings">App that contains settings</param>
        public override void ExecuteProcess(bool SCDebug, bool StageDebug, IAppSettings appSettings, string directory = "", string filename = "")
        {
            GetPacker(appSettings, directory, filename);

            GenerateCode(SCDebug, StageDebug);
            WriteRoot();

            //Gather file need to pack
            Dictionary<string, string> resNeedToPack = new Dictionary<string, string>();
            Dictionary<string, Tuple<string, string>> resPathToMD5 = new Dictionary<string, Tuple<string, string>>();

            if (appSettings.SaveResMeta)
            {
                if (File.Exists(projMetaPath) && Packer.TargetExists())
                {
                    GatherResByResMeta(resNeedToPack, resPathToMD5);
                }
                else
                {
                    GatherResAndSaveMeta(resNeedToPack);
                }
            }
            else
            {
                GatherAllRes(resNeedToPack);
            }

            PackFileUsingInfo(appSettings, resNeedToPack, resPathToMD5, true);

            foreach (PartialProjectProcess process in fileProcess)
            {
                process.ProgressChanged += ProgressChangedEventHandler;
                process.ExecuteProcess(SCDebug, StageDebug, appSettings, directory, filename);
            }
        }
    }
}
