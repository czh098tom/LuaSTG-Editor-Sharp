using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace LuaSTGEditorSharp.Zip
{
    public class ZipCompressorInternal : ZipCompressor
    {
        private readonly string targetArchivePath;
        private ZipFile targetArchive;

        public ZipCompressorInternal(string targetArchivePath)
        {
            Encoding utf8 = Encoding.UTF8;
            ZipStrings.CodePage = utf8.CodePage;
            this.targetArchivePath = targetArchivePath;
        }

        public override void PackByDict(Dictionary<string, string> path, bool removeIfExists)
        {
            HashSet<string> zipNames = new HashSet<string>();
            if (File.Exists(targetArchivePath))
            {
                if (removeIfExists)
                {
                    File.Delete(targetArchivePath);
                    targetArchive = ZipFile.Create(targetArchivePath);
                }
                else
                {
                    targetArchive = new ZipFile(targetArchivePath);
                }
            }
            else
            {
                targetArchive = ZipFile.Create(targetArchivePath);
            }
            foreach(ZipEntry ze in targetArchive)
            {
                zipNames.Add(ze.Name);
                //System.Windows.MessageBox.Show(ze.Name);
            }
            targetArchive.BeginUpdate();
            foreach(KeyValuePair<string,string> kvp in path)
            {
                if (targetArchive.FindEntry(kvp.Key, true) > 0) 
                {
                    targetArchive.Delete(kvp.Key);
                }
                targetArchive.Add(kvp.Value, kvp.Key);
            }
            targetArchive.CommitUpdate();
            //targetArchive.Dispose();
            ((IDisposable)targetArchive).Dispose();
        }
    }
}
