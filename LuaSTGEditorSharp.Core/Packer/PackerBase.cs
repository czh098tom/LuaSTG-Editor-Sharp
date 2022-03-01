using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace LuaSTGEditorSharp.Packer
{
    public abstract class PackerBase
    {
        public static PackerBase GetPacker(string type, string targetPath, string externalToolPath = null, string tempPath = null)
        {
            switch (type)
            {
                case ZipPackerBatch.name:
                    return new ZipPackerBatch(targetPath, externalToolPath, tempPath);
                case PlainCopyPacker.name:
                    return new PlainCopyPacker(targetPath);
                case ZipPackerInternal.name:
                default:
                    return new ZipPackerInternal(targetPath);
            }
        }

        public string TargetArchivePath { get; private set; }

        public virtual bool SupportFolderInArchive { get => true; }

        public PackerBase(string targetPath)
        {
            TargetArchivePath = GetTargetWithExtension(targetPath);
        }

        public abstract bool TargetExists();

        protected abstract string GetTargetWithExtension(string targetPath);

        public virtual void PackByDict(Dictionary<string, string> path, bool removeIfExists)
        {
            try
            {
                foreach (string s in PackByDictReporting(path, removeIfExists)) { }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Packaging failed.\n{e}");
            }
        }

        public abstract IEnumerable<string> PackByDictReporting(Dictionary<string, string> path, bool removeIfExists);
    }
}
