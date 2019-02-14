using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Zip
{
    public class ZipCompressorBatch : ZipCompressor
    {
        private readonly string zipExePath;
        private readonly string targetArchivePath;
        private readonly string batchTempPath;

        public ZipCompressorBatch(string targetArchivePath, string zipExePath, string batchTempPath)
        {
            this.targetArchivePath = targetArchivePath;
            this.zipExePath = zipExePath;
            this.batchTempPath = batchTempPath;
        }

        public override void PackByDict(Dictionary<string, string> fileInfo, bool removeIfExists)
        {
            FileStream packBatS = null;
            StreamWriter packBat = null;
            if (removeIfExists && File.Exists(targetArchivePath)) File.Delete(targetArchivePath);
            try
            {
                packBatS = new FileStream(batchTempPath, FileMode.Create);
                packBat = new StreamWriter(packBatS, Encoding.Default);
                foreach (KeyValuePair<string, string> kvp in fileInfo)
                {
                    packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetArchivePath + "\" \"" + kvp.Value + "\"");
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
