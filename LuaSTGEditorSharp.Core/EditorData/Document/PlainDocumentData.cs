using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Compile;
using LuaSTGEditorSharp.EditorData.Exception;
using LuaSTGEditorSharp.EditorData.Interfaces;

namespace LuaSTGEditorSharp.EditorData.Document
{
    public class PlainDocumentData : DocumentData, IDocumentWithMeta
    {
        public ProjectData parentProj = null;

        public PlainDocumentData(int hash, bool supressMessage = false) : base(hash, supressMessage)
        {
            meta = new MetaDataEntity(this, supressMessage);
        }

        public override string Extension => ".lstges";

        private readonly MetaDataEntity meta;

        public override MetaDataEntity OriginalMeta
        {
            get
            {
                return meta;
            }
        }

        public AbstractMetaData UndecidedMeta { get => meta; }

        public override AbstractMetaData Meta
        {
            get
            {
                if (parentProj == null)
                {
                    return meta;
                }
                else
                {
                    return parentProj.Meta;
                }
            }
        }

        public VirtualDoc GetVirtualDoc()
        {
            return new VirtualDoc()
            {
                DocPath = DocPath,
                UndecidedMeta = meta.GetVirtualized()
            };
        }

        public override void OnOpening()
        {
            foreach(DocumentData dc in parent)
            {
                if(dc is ProjectData pd)
                {
                    foreach(IDocumentWithMeta idwm in pd.referencedDoc)
                    {
                        if (idwm.DocPath == DocPath) 
                        {
                            int id = pd.referencedDoc.IndexOf(idwm);
                            pd.referencedDoc.RemoveAt(id);
                            pd.referencedDoc.Insert(id, this);
                            parentProj = pd;
                            parentProj.OriginalMeta.RaisePropertyChanged("r");
                            //TODO: Throw something indicating a file cannot be referenced by more than one ProjData
                            return;
                        }
                    }
                }
            }
            //System.Windows.MessageBox.Show(parentProj.ToString());
        }

        public override void OnClosing()
        {
            base.OnClosing();
            if (parentProj != null)
            {
                int id = parentProj.referencedDoc.IndexOf(this);
                parentProj.referencedDoc.Remove(this);
                parentProj.referencedDoc.Insert(id, GetVirtualDoc());
                GetVirtualDoc().SaveMeta();
                parentProj.OriginalMeta.RaisePropertyChanged("r");
            }
        }

        public override void GatherCompileInfo(IAppSettings mainAppWithInfo)
        {
            CompileProcess c;
            if (parentProj == null)
            {
                c = new SingleFileProcess();
                if(!CompileProcess.CanOperate(mainAppWithInfo.TempPath))
                {
                    string tempPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "LuaSTG Editor/"));
                    if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
                    c.currentTempPath = tempPath;
                }
                else
                {
                    c.currentTempPath = mainAppWithInfo.TempPath;
                }
                c.projLuaPath = c.currentTempPath + "_editor_output.lua";

                c.projMetaPath = DocPath + ".meta";
            }
            else
            {
                c = new PartialProjectProcess();
                if (!CompileProcess.CanOperate(mainAppWithInfo.TempPath))
                {
                    string tempPath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "LuaSTG Editor/"));
                    if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
                    c.currentTempPath = tempPath;
                }
                else
                {
                    c.currentTempPath = mainAppWithInfo.TempPath;
                }
                (c as PartialProjectProcess).parentProcess = parentProj.CompileProcess as ProjectProcess;
                c.projLuaPath = c.currentTempPath + Path.GetFileNameWithoutExtension(RawDocName) + ".lua";

                c.projMetaPath = DocPath + ".projmeta";
            }
            CompileProcess = c;

            c.source = this;
            c.rootLuaPath = c.currentTempPath + "root.lua";
            c.rootZipPackPath = c.currentTempPath + "pack.bat";

            c.projPath = "";
            if (!string.IsNullOrEmpty(DocPath))
                c.projPath = Path.GetDirectoryName(DocPath);

            c.rootCode = "Include\'THlib.lua\'\nInclude\'_editor_output.lua\'";

            c.zipExePath = mainAppWithInfo.ZipExecutablePath;
            c.luaSTGExePath = mainAppWithInfo.LuaSTGExecutablePath;

            if (!mainAppWithInfo.IsEXEPathSet) throw new EXEPathNotSetException();

            c.projName = Path.GetFileNameWithoutExtension(RawDocName);

            //Find mod name
            foreach (TreeNode t in TreeNodes[0].Children)
            {
                if (t is ProjSettings)
                {
                    if (!string.IsNullOrEmpty(t.attributes[0].AttrInput)) c.projName = t.attributes[0].AttrInput;
                    break;
                }
            }

            c.luaSTGFolder = Path.GetDirectoryName(c.luaSTGExePath);

            if (parentProj == null)
            {
                c.targetZipPath = c.luaSTGFolder + "\\mod\\" + c.projName + ".zip";
            }
            else
            {
                c.targetZipPath = (c as PartialProjectProcess).parentProcess.targetZipPath;
                //System.Windows.MessageBox.Show(c.targetZipPath);
            }

            //System.Windows.MessageBox.Show("fin.");
        }
    }
}
