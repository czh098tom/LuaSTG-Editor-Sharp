using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Node.General;
using LuaSTGEditorSharp.EditorData.Node.Data;
using LuaSTGEditorSharp.EditorData.Node.Stage;
using LuaSTGEditorSharp.EditorData.Node.Task;
using LuaSTGEditorSharp.EditorData.Node.Boss;
using LuaSTGEditorSharp.EditorData.Node.Bullet;
using LuaSTGEditorSharp.EditorData.Node.Object;
using LuaSTGEditorSharp.EditorData.Node.Graphics;
using LuaSTGEditorSharp.EditorData.Node.Audio;
using LuaSTGEditorSharp.EditorData.Node.Advanced;

namespace NodeTest
{
    //[TestClass]
    public class CodeTest : NodeTestBase<Code>
    {
        public override Code NewT(DocumentData d)
        {
            return new Code(d);
        }

        //[TestMethod]
        public override void ToLuaTest()
        {
            AssetToLua("    a\n\n", 1, "a\n");
            ReassetToLua("    a\n    aa\n    aaa\n    aaaa\n\n", 1, "a\naa\naaa\naaaa\n");
        }

        //[TestMethod]
        public override void RowCountTest()
        {
            AssetRowCount(1, "a");
            ReassetRowCount(4, "a\naa\naaa\naaaa");
        }
    }
}
