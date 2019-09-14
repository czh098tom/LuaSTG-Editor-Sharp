using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;

namespace LuaSTGEditorSharp.EditorData.Document
{
    public class VirtualDoc : IDocumentWithMeta
    {
        public string DocPath { get; set; }
        public AbstractMetaData UndecidedMeta { get; set; }

        public void SaveMeta()
        {
            string path = DocPath + ".lstgdef";
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = new FileStream(path, FileMode.Create);
                sw = new StreamWriter(fs);
                sw.Write(EditorSerializer.SerializeMetaData(UndecidedMeta));
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }
            finally
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
        }

        public bool LoadMeta()
        {
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                string path = DocPath + ".lstgdef";
                fs = new FileStream(path, FileMode.Open);
                sr = new StreamReader(fs);
                UndecidedMeta = (AbstractMetaData)EditorSerializer.DeserializeMetaData(sr.ReadToEnd());
                UndecidedMeta.CheckIntegrity();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();
            }
        }
    }
}
