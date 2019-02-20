using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NodeTest
{
    [TestClass]
    public class Miscellaneous
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("1", string.Format("{0}", new object[] { "1", 2, 3 }));
            Assert.AreEqual("1,3", string.Format("{0},{2}", new object[] { "1", 2, 3 }));
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual("LuaSTG Editor", new DirectoryInfo(@"C:\Users\czh\AppData\Local\Temp\LuaSTG Editor").Name);
        }
    }
}
