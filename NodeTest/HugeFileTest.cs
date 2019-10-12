using System;
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SevenZip;

namespace NodeTest
{
    //[TestClass]
    public class HugeFileTest
    {
        /// <summary>
        /// Create a huge random file at D:\Huge_rnd.lstges.
        /// </summary>
        //[TestMethod]
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

        //[TestMethod]
        public void SevenZip2Folder()
        {
            /*
            SevenZipBase.SetLibraryPath(@"D:\7zdll-master\7z\7z.dll");
            SevenZipCompressor szc = new SevenZipCompressor();
            szc.ArchiveFormat = OutArchiveFormat.Zip;
            szc.CompressionMode = CompressionMode.Append;
            szc.CompressFileDictionary(new Dictionary<string, string>() { { @"aaa\啊♂.txt", @"D:\test.tex" } }, @"D:\test.zip");
            
            ZipFile.Create(@"D:\test.zip").Close();
            ZipFile zf = new ZipFile(@"D:\test.zip");
            zf.BeginUpdate();
            zf.Add(@"D:\test.tex", @"aaa\啊♂.txt");
            zf.Close();
            */
            var zipCompressorInternal = new LuaSTGEditorSharp.Zip.ZipCompressorInternal(@"D:\test.zip");
            var dict = new Dictionary<string, string>
            {
                { @"aaa\啊♂.txt", @"D:\test.tex" }
            };
            zipCompressorInternal.PackByDict(dict, true);
        }
    }
}
