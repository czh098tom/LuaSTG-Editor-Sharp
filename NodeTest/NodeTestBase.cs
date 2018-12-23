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
    public abstract class NodeTestBase<T> where T : TreeNode
    {
        protected DocumentData document = new PlainDocumentData(0);
        private T temp;

        public abstract T NewT(DocumentData d);

        private void InitDoc()
        {
            document = new PlainDocumentData(0);
        }

        protected void Assign(params string[] assignments)
        {
            for (int i = 0; i < assignments.Length; i++)
            {
                temp.attributes[i].AttrInput = assignments[i];
            }
        }

        protected void Assign(int i, string assignment)
        {
            temp.attributes[i].AttrInput = assignment;
        }

        public void AssetToLua(string exp, int sp, params string[] assignments)
        {
            InitDoc();
            temp = NewT(document);
            ReassetToLua(exp, sp, assignments);
        }

        public void AssetToLua(string exp, int sp, int i, string assignment)
        {
            InitDoc();
            NewT(document);
            ReassetToLua(exp, sp, i, assignment);
        }

        public void ReassetToLua(string exp, int sp, params string[] assignments)
        {
            Assign(assignments);
            Assert.AreEqual(exp, temp.ToLua(sp));
        }

        public void ReassetToLua(string exp, int sp, int i, string assignment)
        {
            Assign(i, assignment);
            Assert.AreEqual(exp, temp.ToLua(sp));
        }

        public void AssetRowCount(int exp, params string[] assignments)
        {
            InitDoc();
            NewT(document);
            ReassetRowCount(exp, assignments);
        }

        public void AssetRowCount(int exp, int i, string assignment)
        {
            InitDoc();
            NewT(document);
            ReassetRowCount(exp, i, assignment);
        }

        public void ReassetRowCount(int exp, int i, string assignment)
        {
            Assign(i, assignment);
            Assert.AreEqual(exp, temp.GetLines());
        }

        public void ReassetRowCount(int exp, params string[] assignments)
        {
            Assign(assignments);
            Assert.AreEqual(exp, temp.GetLines());
        }

        public abstract void ToLuaTest();
        public abstract void RowCountTest();
    }
}
