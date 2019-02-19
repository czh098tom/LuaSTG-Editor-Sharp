using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NodeTest
{
    [TestClass]
    public class StringDotFormatTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("1", string.Format("{0}", new object[] { "1", 2, 3 }));
            Assert.AreEqual("1,3", string.Format("{0},{2}", new object[] { "1", 2, 3 }));
        }
    }
}
