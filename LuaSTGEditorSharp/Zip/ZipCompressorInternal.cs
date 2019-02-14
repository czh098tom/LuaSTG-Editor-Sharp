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
            try
            {
                foreach (string s in PackByDictReporting(path, removeIfExists)) { }
            }
            catch
            {
                System.Windows.MessageBox.Show("Packaging failed.");
            }
        }

        public override IEnumerable<string> PackByDictReporting(Dictionary<string, string> path, bool removeIfExists)
        {
            HashSet<string> zipNames = new HashSet<string>();
            try
            {
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
            }
            catch
            {
                System.Windows.MessageBox.Show("Packaging failed.");
                yield break;
            }
            /*
            foreach (ZipEntry ze in targetArchive)
            {
                zipNames.Add(ze.Name);
                //System.Windows.MessageBox.Show(ze.Name);
            }
            targetArchive.BeginUpdate();
            foreach (KeyValuePair<string, string> kvp in path)
            {
                if (targetArchive.FindEntry(kvp.Key, true) > 0)
                {
                    targetArchive.Delete(kvp.Key);
                }
            }
            targetArchive.CommitUpdate();
            */
            //targetArchive.Close();
            ((IDisposable)targetArchive).Dispose();

            using (ZipOutputStream ZipStream = new ZipOutputStream(new FileStream(targetArchivePath, FileMode.Open)))
            {
                foreach (KeyValuePair<string, string> kvp in path)
                {
                    using (FileStream StreamToZip = new FileStream(kvp.Value, FileMode.Open, FileAccess.Read))
                    {
                        ZipEntry ZipEntry = new ZipEntry(kvp.Key);
                        
                        ZipStream.PutNextEntry(ZipEntry);
                        ZipStream.Flush();

                        byte[] buffer = new byte[2048];

                        int sizeRead = 0;

                        try
                        {
                            do
                            {
                                sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        StreamToZip.Close();
                    }
                    yield return $"Add file \"{kvp.Value}\" into archive, internal name: \"{kvp.Key}\"";
                }
                ZipStream.Finish();
                ZipStream.Close();
            }
        }

        public IEnumerable<string> PackByDictReporting_old(Dictionary<string, string> path, bool removeIfExists)
        {
            HashSet<string> zipNames = new HashSet<string>();
            try
            {
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
            }
            catch
            {
                System.Windows.MessageBox.Show("Packaging failed.");
                yield break;
            }
            foreach (ZipEntry ze in targetArchive)
            {
                zipNames.Add(ze.Name);
                //System.Windows.MessageBox.Show(ze.Name);
            }
            targetArchive.BeginUpdate();
            foreach (KeyValuePair<string, string> kvp in path)
            {
                if (targetArchive.FindEntry(kvp.Key, true) > 0)
                {
                    targetArchive.Delete(kvp.Key);
                }
                targetArchive.Add(kvp.Value, kvp.Key);
                yield return $"Add file \"{kvp.Value}\" in to zip.";
            }
            targetArchive.CommitUpdate();
            ((IDisposable)targetArchive).Dispose();
        }
    }
}
