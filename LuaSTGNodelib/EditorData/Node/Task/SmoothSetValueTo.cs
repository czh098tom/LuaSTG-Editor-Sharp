using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("tasksetvalue.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [IgnoreAttributesParityCheck]
    public class SmoothSetValueTo : TreeNode
    {
        [JsonConstructor]
        private SmoothSetValueTo() : base() { }

        public SmoothSetValueTo(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Target", "self", this, "nullabletarget"));
            attributes.Add(new AttrItem("Waiting time", "0", this));
            attributes.Add(new AttrItem("Duration", "60", this));
            attributes.Add(new DependencyAttrItem("Number of Value", "1", this));
            attributes.Add(new AttrItem("Value 1 name", "_speed", this, "prop"));
            attributes.Add(new AttrItem("Target value 1", "1", this));
            attributes.Add(new AttrItem("Interpolation mode 1", "MOVE_NORMAL", this, "interpolation"));
            attributes.Add(new AttrItem("Modification mode 1", "MODE_SET", this, "modification"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            if (!int.TryParse(NonMacrolize(3), out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;

            string tar = Macrolize(0);
            string wt = Macrolize(1);
            wt = string.IsNullOrEmpty(wt) ? "0" : wt;
            string dur = Macrolize(2);
            dur = string.IsNullOrEmpty(dur) ? "1" : dur;

            string valT;

            if (!string.IsNullOrEmpty(tar))
            {
                for (int i = 4; i <= nAttr * 4; i += 4)
                {
                    valT = Macrolize(i + 1);
                    if (!string.IsNullOrEmpty(valT))
                    {
                        yield return "lasttask=task.New(" + tar + ",function () ex.SmoothSetValueTo(\""
                            + Lua.StringParser.ParseLua(NonMacrolize(i)) + "\"," + valT + "," + dur
                            + "," + Macrolize(i + 2) + ",nil," + wt + "," + Macrolize(i + 3) + ") end )\n";
                    }
                }
            }
            else
            {
                string valN;
                for (int i = 4; i <= nAttr * 4; i += 4)
                {
                    valT = Macrolize(i + 1);
                    if (!string.IsNullOrEmpty(valT))
                    {
                        valN = Macrolize(i);
                        yield return "lasttask=task.New(self,function () ex.SmoothSetValueTo(function() return "
                            + valN + " end," + valT + "," + dur + "," + Macrolize(i + 2) + ",function(___) " 
                            + valN + "=___ end," + wt + "," + Macrolize(i + 3) + ") end )\n";
                    }
                }
            }
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            int count = 0;
            if (!int.TryParse(NonMacrolize(3), out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 4; i <= nAttr * 4; i += 4)
            {
                if (!string.IsNullOrEmpty(NonMacrolize(i + 1)))
                {
                    count++;
                }
            }
            yield return new Tuple<int, TreeNode>(count, this);
        }

        public override string ToString()
        {
            string bres;
            if (!string.IsNullOrEmpty(NonMacrolize(0)))
            {
                bres = (NonMacrolize(1) == "0" ? "S" : "Wait " + NonMacrolize(1) + " frame(s), s")
                    + "mooth set value in object \"" + NonMacrolize(0) + "\" in " + NonMacrolize(2) + " frame(s):";
            }
            else
            {
                bres = (NonMacrolize(1) == "0" ? "S" : "Wait " + NonMacrolize(1) + " frame(s), s")
                    + "mooth set value in " + NonMacrolize(2) + " frame(s):";
            }
            if (!int.TryParse(NonMacrolize(3), out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 4; i <= nAttr * 4; i += 4)
            {
                if (!string.IsNullOrEmpty(NonMacrolize(i + 1)))
                {
                    bres += "\n" + NonMacrolize(i) + " => " + NonMacrolize(i + 1);
                }
            }
            return bres;
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            int n = (attributes.Count) / 4 - 1;
            if (!int.TryParse(NonMacrolize(3), out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            if (n != nAttr)
            {
                if (n < nAttr)
                {
                    for (int i = n + 1; i <= nAttr; i++)
                    {
                        attributes.Add(new AttrItem("Value " + i + " name", "_speed", this, "prop"));
                        attributes.Add(new AttrItem("Target value " + i, "1", this));
                        attributes.Add(new AttrItem("Interpolation mode " + i, "MOVE_NORMAL", this, "interpolation"));
                        attributes.Add(new AttrItem("Modification mode " + i, "MODE_SET", this, "modification"));
                    }
                }
                else
                {
                    for (int i = 0; i < 4 * (n - nAttr); i++)
                    {
                        attributes.RemoveAt(nAttr * 4 + 4);
                    }
                }
            }
        }

        public override object Clone()
        {
            var n = new SmoothSetValueTo(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (!int.TryParse(NonMacrolize(3), out int nAttr)) nAttr = 0;
            if (nAttr > AppConstants.mxUAttr)
            {
                messages.Add(new ArgCountInfo(attributes[0].AttrCap, true, this));
                nAttr = AppConstants.mxUAttr;
            }
            if (nAttr <= 0) messages.Add(new ArgCountInfo(attributes[0].AttrCap, false, this));
            return messages;
        }
    }
}
