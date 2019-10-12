using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace NodeTest
{
    [TestClass]
    public class NodeParityCheckTest
    {
        [TestMethod]
        public void Add1()
        {
            var n = new Imaginary.ImaginaryNode();
            n.attributes.Add(new AttrItem("A", "1", n));
            n.FixAttributesList();
            Assert.AreEqual("A", n.attributes[0].AttrCap);
            Assert.AreEqual("1", n.attributes[0].attrInput);
            Assert.AreEqual("C", n.attributes[1].AttrCap);
            Assert.AreEqual("", n.attributes[1].attrInput);
        }

        [TestMethod]
        public void Remove1()
        {
            var n = new Imaginary.ImaginaryNode();
            n.attributes.Add(new AttrItem("A", "1", n));
            n.attributes.Add(new AttrItem("B", "2", n));
            n.attributes.Add(new AttrItem("C", "3", n));
            n.FixAttributesList();
            Assert.AreEqual("A", n.attributes[0].AttrCap);
            Assert.AreEqual("1", n.attributes[0].attrInput);
            Assert.AreEqual("C", n.attributes[1].AttrCap);
            Assert.AreEqual("3", n.attributes[1].attrInput);
        }
    }
}
