using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Packer
{
    public class ZipPackerBatch : PackerBase
    {
        public const string name = "zip-external";

        private readonly string zipExePath;
        private readonly string batchTempPath;

        public override bool SupportFolderInArchive { get => false; }

        public ZipPackerBatch(string targetArchivePath, string zipExePath, string batchTempPath) : base(targetArchivePath)
        {
            this.zipExePath = zipExePath;
            this.batchTempPath = batchTempPath;
        }

        protected override string GetTargetWithExtension(string targetPath)
        {
            return targetPath + ".zip";
        }

        public override bool TargetExists()
        {
            return File.Exists(TargetArchivePath);
        }

        public override void PackByDict(Dictionary<string, string> fileInfo, bool removeIfExists)
        {
            FileStream packBatS = null;
            StreamWriter packBat = null;
            if (removeIfExists && TargetExists()) File.Delete(TargetArchivePath);
            try
            {
                packBatS = new FileStream(batchTempPath, FileMode.Create);
                packBat = new StreamWriter(packBatS, Encoding.Default);
                foreach (KeyValuePair<string, string> kvp in fileInfo)
                {
                    packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + TargetArchivePath + "\" \"" + kvp.Value + "\"");
                }
                packBat.Close();
                packBatS.Close();
                Process pack = new Process
                {
                    StartInfo = new ProcessStartInfo(batchTempPath)
                    {
                        UseShellExecute = true,
                        CreateNoWindow = false
                    }
                };
                pack.Start();
                pack.WaitForExit();
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }
            finally
            {
                if (packBat != null) packBat.Close();
                if (packBatS != null) packBatS.Close();
            }
        }

        public override IEnumerable<string> PackByDictReporting(Dictionary<string, string> path, bool removeIfExists)
        {
            PackByDict(path, removeIfExists);
            yield break;
        }
    }
}
