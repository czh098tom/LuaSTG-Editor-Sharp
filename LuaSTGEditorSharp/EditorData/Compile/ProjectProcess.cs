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

namespace LuaSTGEditorSharp.EditorData.Compile
{
    internal class ProjectProcess : CompileProcess
    {
        internal readonly List<PartialProjectProcess> fileProcess = new List<PartialProjectProcess>();

        internal override void ExecuteProcess(bool SCDebug, bool StageDebug)
        {
            App currentApp = Application.Current as App;

            GenerateCode(SCDebug, StageDebug);
            WriteRoot();

            //Gather file need to pack
            List<string> resNeedToPack = new List<string>();
            Dictionary<string, string> resPathToMD5 = new Dictionary<string, string>();

            if (currentApp.SaveResMeta)
            {
                if (File.Exists(projMetaPath) && File.Exists(targetZipPath))
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

            PackFileUsingInfo(currentApp, resNeedToPack, resPathToMD5, true);

            foreach (PartialProjectProcess process in fileProcess)
            {
                process.ExecuteProcess(SCDebug, StageDebug);
            }
        }
    }
}
