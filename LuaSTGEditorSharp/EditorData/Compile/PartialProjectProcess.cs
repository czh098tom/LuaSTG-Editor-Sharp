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
    /// <summary>
    /// The <see cref="CompileProcess"/> of part of a luastg project.
    /// </summary>
    internal class PartialProjectProcess :  CompileProcess
    {
        /// <summary>
        /// The reference of parent <see cref="ProjectProcess"/> of parent <see cref="ProjectData"/>.
        /// </summary>
        internal ProjectProcess parentProcess;
        
        /// <summary>
        /// Execute the <see cref="CompileProcess"/>.
        /// </summary>
        /// <param name="SCDebug">Whether SCDebug is switched on.</param>
        /// <param name="StageDebug">Whether Stage Debug is switched on.</param>
        internal override void ExecuteProcess(bool SCDebug, bool StageDebug)
        {
            App currentApp = Application.Current as App;

            GenerateCode(SCDebug, StageDebug);

            //Gather file need to pack
            Dictionary<string, string> resNeedToPack = new Dictionary<string, string>();
            Dictionary<string, Tuple<string, string>> resPathToMD5 = new Dictionary<string, Tuple<string, string>>();

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

            PackFileUsingInfo(currentApp, resNeedToPack, resPathToMD5, false, true);
        }
    }
}
