using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using LuaSTGEditorSharp.EditorData;

namespace NodeTest
{
    [TestClass]
    public class NodeAttributeValidationTest
    {
        [TestMethod]
        public void NameChange()
        {
            TreeNode t = Generate();
            t.DoubleCheckAttr(0, name: "a");
            t.DoubleCheckAttr(1, name: "b");
            Assert.AreEqual("a", t.attributes[0].AttrCap);
            Assert.AreEqual("1", t.attributes[0].attrInput);
            Assert.AreEqual("b", t.attributes[1].AttrCap);
            Assert.AreEqual("2", t.attributes[1].attrInput);
        }

        [TestMethod]
        public void Move()
        {
            TreeNode t = Generate();
            t.DoubleCheckAttr(0, name: "a");
            t.DoubleCheckAttr(2, name: "c");
            Assert.AreEqual("a", t.attributes[0].AttrCap);
            Assert.AreEqual("1", t.attributes[0].attrInput);
            Assert.AreEqual(null, t.attributes[1]);
            Assert.AreEqual("c", t.attributes[2].AttrCap);
            Assert.AreEqual("2", t.attributes[2].attrInput);
        }

        [TestMethod]
        public void Insert()
        {
            TreeNode t = Generate();
            t.DoubleCheckAttr(0, name: "a");
            t.DoubleCheckAttr(1, name: "b");
            t.DoubleCheckAttr(2, name: "c");
            Assert.AreEqual("a", t.attributes[0].AttrCap);
            Assert.AreEqual("1", t.attributes[0].attrInput);
            Assert.AreEqual("b", t.attributes[1].AttrCap);
            Assert.AreEqual("2", t.attributes[1].attrInput);
            Assert.AreEqual("c", t.attributes[2].AttrCap);
            Assert.AreEqual("", t.attributes[2].attrInput);
        }

        [TestMethod]
        public void Rearrange()
        {
            TreeNode t = Generate();
            t.DoubleCheckAttr(0, name: "b");
            t.DoubleCheckAttr(1, name: "c");
            t.DoubleCheckAttr(2, name: "a");
            Assert.AreEqual("b", t.attributes[0].AttrCap);
            Assert.AreEqual("1", t.attributes[0].attrInput);
            Assert.AreEqual("c", t.attributes[1].AttrCap);
            Assert.AreEqual("2", t.attributes[1].attrInput);
            Assert.AreEqual("a", t.attributes[2].AttrCap);
            Assert.AreEqual("", t.attributes[2].attrInput);
        }

        private static TreeNode Generate()
        {
            TreeNode t = new Imaginary.ImaginaryNode();
            t.attributes.Add(new AttrItem("a", "1", t));
            t.attributes.Add(new AttrItem("c", "2", t));
            return t;
        }
    }
}
