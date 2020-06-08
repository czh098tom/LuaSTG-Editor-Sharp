using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Node.Bullet;
using LuaSTGEditorSharp.Windows;

namespace DanmakuRandomizer.Model
{
    class SimpleBullet : Node
    {
        public List<string> Velocity { get; set; } = new List<string>() { "2" };
        public List<string> Angle { get; set; } = new List<string>() { "0" };
        public List<string> PositionX { get; set; } = new List<string>() { "self.x" };
        public List<string> PositionY { get; set; } = new List<string>() { "self.y" };

        public override string Text => throw new NotImplementedException();

        public override TreeNode GetTreeNode(DocumentData documentData)
        {
            return new CreateSimpleBullet(documentData,
                InputWindowSelector.SelectComboBox("bulletStyle")[0],
                InputWindowSelector.SelectComboBox("color")[0],
                string.Join("+", PositionX.Select((s) => $"({s})")) + "," + string.Join("+", PositionY.Select((s) => $"({s})")),
                string.Join("*", Velocity.Select((s) => $"({s})")),
                string.Join("+", Angle.Select((s) => $"({s})")),
                "false",
                "0",
                "false",
                "true",
                "0",
                "false",
                "0",
                "0",
                "0",
                "false");
        }
    }
}
