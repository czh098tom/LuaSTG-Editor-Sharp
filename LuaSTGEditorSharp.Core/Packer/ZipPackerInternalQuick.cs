using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace LuaSTGEditorSharp.Packer
{
    public class ZipPackerInternalQuick : PackerBase
    {
        public const string name = "zip-internal-quick";

        private FileStream targetArchiveFS;
        private ZipFile targetArchive;

        public ZipPackerInternalQuick(string targetArchivePath) : base(targetArchivePath)
        {
            ZipStrings.UseUnicode = true;
        }

        protected override string GetTargetWithExtension(string targetPath)
        {
            return targetPath + ".zip";
        }

        public override bool TargetExists()
        {
            return File.Exists(TargetArchivePath);
        }

        public override IEnumerable<string> PackByDictReporting(Dictionary<string, string> path, bool removeIfExists)
        {
            HashSet<string> zipNames = new HashSet<string>();
            try
            {
                if (TargetExists())
                {
                    if (removeIfExists)
                    {
                        File.Delete(TargetArchivePath);
                        targetArchive = ZipFile.Create(TargetArchivePath);
                    }
                    else
                    {
                        targetArchive = new ZipFile(TargetArchivePath);
                    }
                }
                else
                {
                    targetArchive = ZipFile.Create(TargetArchivePath);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Packaging failed.\n{e}");
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
                yield return $"Adding file \"{kvp.Value}\" in to zip.";
                if (targetArchive.FindEntry(kvp.Key, true) > 0)
                {
                    targetArchive.Delete(kvp.Key);
                }
                targetArchive.Add(kvp.Value, kvp.Key);
                yield return $"Added file \"{kvp.Value}\" in to zip.";
            }
            targetArchive.CommitUpdate();
            ((IDisposable)targetArchive).Dispose();
        }

        //Problem occurs when updating
        public IEnumerable<string> PackByDictReporting_old(Dictionary<string, string> path, bool removeIfExists)
        {
            HashSet<string> zipNames = new HashSet<string>();
            try
            {
                if (TargetExists())
                {
                    if (removeIfExists)
                    {
                        File.Delete(TargetArchivePath);
                        targetArchiveFS = File.Create(TargetArchivePath);
                    }
                    else
                    {
                        targetArchiveFS = new FileStream(TargetArchivePath, FileMode.Open);
                    }
                }
                else
                {
                    targetArchiveFS = File.Create(TargetArchivePath);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Packaging failed.\n{e}");
                yield break;
            }
            //targetArchiveFS.Close();
            //((IDisposable)targetArchiveFS).Dispose();

            using (ZipOutputStream ZipStream = new ZipOutputStream(targetArchiveFS))
            {
                foreach (KeyValuePair<string, string> kvp in path)
                {
                    using (FileStream StreamToZip = new FileStream(kvp.Value, FileMode.Open, FileAccess.Read))
                    {
                        yield return $"Add file \"{kvp.Value}\" into archive \"{TargetArchivePath}\", internal name: \"{kvp.Key}\".";
                        ZipEntry ZipEntry = new ZipEntry(kvp.Key);

                        ZipStream.PutNextEntry(ZipEntry);
                        ZipStream.Flush();

                        byte[] buffer = new byte[2048];

                        int sizeRead = 0;

                        do
                        {
                            sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                            ZipStream.Write(buffer, 0, sizeRead);
                        }
                        while (sizeRead > 0);
                        StreamToZip.Close();
                    }
                }
                ZipStream.Finish();
                ZipStream.Close();
                yield return $"Finished archive \"{TargetArchivePath}\".";
            }
            targetArchiveFS.Close();
        }

    }
}
