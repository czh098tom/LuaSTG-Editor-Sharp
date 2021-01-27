using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Zip
{
    public class PlainCopy : ZipCompressor
    {
        public override void PackByDict(Dictionary<string, string> path, bool removeIfExists)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> PackByDictReporting(Dictionary<string, string> path, bool removeIfExists)
        {
            throw new NotImplementedException();
        }
    }
}
