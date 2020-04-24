using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Compile
{
    /// <summary>
    /// Class to store macro definitions.
    /// </summary>
    public class DefineMarcoSettings
    {
        /// <summary>
        /// The <see cref="string"/> to search.
        /// </summary>
        public string ToBeReplaced { get; set; }
        /// <summary>
        /// The <see cref="string"/> to replace.
        /// </summary>
        public string New { get; set; }
    }
}
