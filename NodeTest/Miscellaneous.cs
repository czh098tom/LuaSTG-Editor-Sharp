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

        [TestMethod]
        public void TestMethod3()
        {
            Assert.AreEqual(true, Directory.Exists(@"C:\Users\czh\AppData\Local\Temp\LuaSTG Editor"));
            Assert.AreEqual(false, Directory.Exists(@"C:\Users\czh\AppData\Local\Temp\LuaSTG Editor\log.txt"));
        }

        [TestMethod]
        public void TestMethod4()
        {
            Assert.AreEqual(".wav", Path.GetExtension(@"pack://application:,,,/LuaSTGNodeLib;component/se/se_bonus.wav"));
        }

        [TestMethod]
        public void GenerateWords()
        {
            string[] strs = new string[]{ "alert", "astralup", "bonus", "bonus2", "boon00", "boon01", "cancel00"
                        , "cardget", "cat00", "cat01", "ch00", "ch01", "ch02", "don00", "damage00", "damage01"
                        , "enep00", "enep01", "enep02", "extend", "fault", "graze", "gun00", "hint00", "invalid"
                        , "item00", "kira00", "kira01", "kira02", "lazer00", "lazer01", "lazer02", "msl", "msl2"
                        , "nep00", "ok00", "option", "pause", "pldead00", "plst00", "power0", "power1", "powerup"
                        , "select00", "slash", "tan00", "tan01", "tan02", "timeout", "timeout2","warpl", "warpr"
                        , "water", "explode", "nice", "nodamage", "power02", "lgods1", "lgods2", "lgods3", "lgods4"
                        , "lgodsget", "big", "wolf", "noise", "pin00", "powerup1", "old_cat00", "old_enep00"
                        , "old_extend", "old_gun00", "old_kira00", "old_kira01", "old_lazer01", "old_nep00"
                        , "old_pldead00", "old_power0", "old_power1", "old_powerup", "hyz_charge00", "hyz_charge01b"
                        , "hyz_chargeup", "hyz_eterase", "hyz_exattack", "hyz_gosp", "hyz_life1", "hyz_playerdead"
                        , "hyz_timestop0", "hyz_warning", "bonus3", "border", "changeitem", "down", "extend2"
                        , "focusfix", "focusfix2", "focusin", "heal", "ice", "ice2", "item01", "ophide", "opshow" };
            var mms = new System.Collections.Generic.List<string>();
            foreach (string s in strs)
            {

                mms.Add("  {\n    \"Icon\": \"/LuaSTGNodeLib;component/images/16x16/loadsound.png\"\n"
                    + "    \"Text\": \"(Internal) " + s + "\"\n"
                    + "    \"Result\": \"\\\"" + s + "\\\"\"\n"
                    + "    \"FullName\": \"" + s + "\"\n"
                    + "    \"ExInfo1\": \"pack://application:,,,/LuaSTGNodeLib;component/se/se_" + s + ".wav\"\n"
                    + "  }");
            }
            System.IO.StreamWriter sw = new StreamWriter("D:/aa.json");
            sw.WriteLine("[");
            string sfull = string.Join(",\n", mms);
            sw.WriteLine(sfull);
            sw.WriteLine("]");
            sw.Close();
        }
    }
}
