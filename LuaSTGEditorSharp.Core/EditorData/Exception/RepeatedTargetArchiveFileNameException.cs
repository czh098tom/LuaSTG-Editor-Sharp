using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Exception
{
    using Exception = System.Exception;

    class RepeatedTargetArchiveFileNameException : Exception
    {
        public string MismatchedName { get; set; }

        public RepeatedTargetArchiveFileNameException(string name)
            : base($"Repeated resource \"{name}\" in archive.") { }
        public RepeatedTargetArchiveFileNameException(string name, Exception innerException)
            : base($"Repeated resource \"{name}\" in archive.", innerException) { }
    }
}
