using System;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LuaSTGEditorSharp.Windows.Input;

namespace NodeTest
{
    //[TestClass]
    public class ColorTest
    {
        //[TestMethod]
        public void HSVToRGB()
        {
            ARGBInput.HSVColor hsv = new ARGBInput.HSVColor(306, .69f, .71f);
            Color rgb = ARGBInput.HSVToRGB(hsv);
            Assert.AreEqual(181, rgb.R);
            Assert.AreEqual(56, rgb.G);
            Assert.AreEqual(169, rgb.B);
        }

        //[TestMethod]
        public void RGBToHSV()
        {
            Color rgb = Color.FromArgb(255, 255, 255, 255);
            ARGBInput.HSVColor hsv = ARGBInput.RGBToHSV(rgb);
            Assert.AreEqual(306, hsv.hue);
            Assert.AreEqual(.69f, hsv.saturation);
            Assert.AreEqual(.71f, hsv.value);
        }
    }
}
