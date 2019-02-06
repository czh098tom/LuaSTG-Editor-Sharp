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
        private readonly string path;
        private readonly string batchTempPath;

        public ZipCompressorBatch(string path, string zipExePath, string batchTempPath)
        {
            this.path = path;
            this.zipExePath = zipExePath;
            this.batchTempPath = batchTempPath;
        }

        public override void PackByDict(Dictionary<string, string> fileInfo, bool removeIfExists)
        {
            FileStream packBatS = null;
            StreamWriter packBat = null;
            if (removeIfExists && File.Exists(path)) File.Delete(path);
            try
            {
                packBatS = new FileStream(batchTempPath, FileMode.Create);
                packBat = new StreamWriter(packBatS, Encoding.Default);
                foreach (KeyValuePair<string, string> kvp in fileInfo)
                {
                    packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + path + "\" \"" + kvp.Value + "\"");
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

        public override void Dispose()
        {

        }
    }
}
