using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Packer
{
    public class PlainCopyPacker : PackerBase
    {
        public const string name = "copy";

        public PlainCopyPacker(string targetArchivePath) : base(targetArchivePath) { }

        protected override string GetTargetWithExtension(string targetPath)
        {
            return targetPath;
        }

        public override bool TargetExists()
        {
            return Directory.Exists(TargetArchivePath);
        }

        public override IEnumerable<string> PackByDictReporting(Dictionary<string, string> path, bool removeIfExists)
        {
            if (TargetExists())
            {
                if (removeIfExists)
                {
                    Directory.Delete(TargetArchivePath, true);
                    Directory.CreateDirectory(TargetArchivePath);
                }
            }
            else
            {
                Directory.CreateDirectory(TargetArchivePath);
            }
            foreach (KeyValuePair<string, string> kvp in path)
            {
                string tar = Path.Combine(TargetArchivePath, kvp.Key);
                if (File.Exists(tar)) File.Delete(tar);
                Directory.CreateDirectory(Path.GetDirectoryName(tar));
                File.Copy(kvp.Value, tar);
                yield return $"Copied file from \"{kvp.Value}\" to \"{tar}\".";
            }
        }
    }
}
