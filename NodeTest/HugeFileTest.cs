using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NodeTest
{
    [TestClass]
    public class HugeFileTest
    {
        /// <summary>
        /// Create a huge random file at D:\Huge_rnd.lstges.
        /// </summary>
        [TestMethod]
        public void CreateHugeFile()
        {
            FileStream fs = new FileStream("D:\\Huge_rnd.lstges", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            int last = 1;
            Random random = new Random();
            sw.WriteLine("0,{\"$type\":\"LuaSTGEditorSharp.EditorData.Node.RootFolder, LuaSTGEditorSharp\",\"attributes\":[{\"attrCap\":\"Name\",\"attrInput\":\"File\",\"EditWindow\":\"\"}],\"IsExpanded\":true,\"IsSelected\":false}");
            sw.WriteLine("1,{\"$type\":\"LuaSTGEditorSharp.EditorData.Node.ProjSettings, LuaSTGEditorSharp\",\"attributes\":[{\"attrCap\":\"Output Name\",\"attrInput\":\"\",\"EditWindow\":\"\"},{\"attrCap\":\"Author\",\"attrInput\":\"LuaSTG User\",\"EditWindow\":\"\"},{\"attrCap\":\"Allow practice\",\"attrInput\":\"true\",\"EditWindow\":\"bool\"},{\"attrCap\":\"Allow sc practice\",\"attrInput\":\"true\",\"EditWindow\":\"bool\"}],\"IsExpanded\":true,\"IsSelected\":false}");
            sw.WriteLine("1,{\"$type\":\"LuaSTGEditorSharp.EditorData.Node.EditorVersion, LuaSTGEditorSharp\",\"attributes\":[{\"attrCap\":\"Editor version\",\"attrInput\":\"0.0.4.0\",\"EditWindow\":\"\"}],\"IsExpanded\":true,\"IsSelected\":false}");
            for(int i = 0; i < 65536; i++)
            {
                sw.WriteLine(last + ",{\"$type\":\"LuaSTGEditorSharp.EditorData.Node.General.Folder, LuaSTGEditorSharp\",\"attributes\":[{\"attrCap\":\"Name\",\"attrInput\":\""+ Guid.NewGuid().ToString() + "\",\"EditWindow\":\"\"}],\"IsExpanded\":true,\"IsSelected\":false}");
                last = random.Next(1, last + 2);
            }
            sw.Close();
            fs.Close();
        }
    }
}
