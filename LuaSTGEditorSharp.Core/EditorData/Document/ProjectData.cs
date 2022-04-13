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

using System.Windows;

namespace LuaSTGEditorSharp.EditorData.Document
{
    public class ProjectData : DocumentData
    {
        public ObservableCollection<IDocumentWithMeta> referencedDoc = new ObservableCollection<IDocumentWithMeta>();
        private readonly MetaDataEntity meta;

        public override string Extension => ".lstgproj";

        public override string ExtensionInfo
        {
            get
            {
                return "LuaSTG Sharp Project (*.lstgproj)|*.lstgproj";
            }
        }

        private AbstractMetaData[] GetAllMetaData()
        {
            var s = from IDocumentWithMeta id
                    in referencedDoc
                    select id.UndecidedMeta;
            var L = new List<AbstractMetaData>(s)
            {
                OriginalMeta
            };
            return L.ToArray();
            /*
            if (s.Count<AbstractMetaData>() > 0)
            {
                return new List<AbstractMetaData>(s).ToArray();
            }
            else
            {
                return new AbstractMetaData[1];
            }
            */
        }

        public override MetaDataEntity OriginalMeta
        {
            get
            {
                return meta;
            }
        }

        public override AbstractMetaData Meta
        {
            get
            {
                return MetaDataEntity.Combine(GetAllMetaData());
            }
        }

        public ProjectData(int hash, bool supressMessage = false) : base(hash, supressMessage)
        {
            meta = new MetaDataEntity(this, supressMessage);
        }

        public override void OnOpening()
        {
            foreach(MetaInfo mi in OriginalMeta.ProjFileData)
            {
                string s = null;
                bool? undcPath = RelativePathConverter.IsRelativePath(mi.FullName);
                if (undcPath == true)
                {
                    s = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(DocPath), mi.FullName));
                }
                else if(undcPath == false)
                {
                    s = Path.GetFullPath(mi.FullName);
                }
                if (undcPath != null)
                {
                    bool find = false;
                    foreach (DocumentData doc in parent)
                    {
                        if (s == doc.DocPath && doc is PlainDocumentData plainDocument)
                        {
                            find = true;
                            referencedDoc.Add(plainDocument);
                            plainDocument.parentProj = this;
                            plainDocument.OriginalMeta.RaisePropertyChanged("n");
                            break;
                        }
                    }
                    if (!find)
                    {
                        try
                        {
                            VirtualDoc pdd = new VirtualDoc { DocPath = s };
                            if (!pdd.LoadMeta())
                            {
                                DocumentData newDoc = GetNewByExtension(Path.GetExtension(s), -1
                                    , Path.GetFileNameWithoutExtension(s), s);
                                TreeNodeBase t = newDoc.CreateNodeFromFile(s);
                                newDoc.TreeNodes.Add(t);
                                t.RaiseCreate(new OnCreateEventArgs() { parent = null });
                                pdd = (newDoc as PlainDocumentData)?.GetVirtualDoc();
                            }
                            referencedDoc.Add(pdd);
                        }
                        catch { }
                    }
                }
            }
            foreach(MetaInfo mi in OriginalMeta.ProjFileData) {  }
        }

        public override void OnEditing(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == typeof(Node.Project.ProjectFile).ToString() || 
                args.PropertyName == typeof(Meta.ProjectFileMetaInfo).ToString())
            {
                referencedDoc = new ObservableCollection<IDocumentWithMeta>();
                OnOpening();
            }
        }

        public override void OnClosing()
        {
            base.OnClosing();
            foreach(IDocumentWithMeta doc in referencedDoc)
            {
                if (doc is PlainDocumentData)
                {
                    PlainDocumentData pdd = doc as PlainDocumentData;
                    pdd.parentProj = null;
                    pdd.GetVirtualDoc().SaveMeta();
                    pdd.OriginalMeta.RaisePropertyChanged("n");
                }
                else if(doc is VirtualDoc)
                {
                    (doc as VirtualDoc).SaveMeta();
                }
            }
        }

        public override void GatherCompileInfo(IAppSettings mainAppWithInfo)
        {
            ProjectProcess c;
            c = new ProjectProcess();
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
            CompileProcess = c;

            c.projLuaPath = c.currentTempPath + "_editor_output.lua";

            c.source = this;
            c.rootLuaPath = c.currentTempPath + "root.lua";
            c.rootZipPackPath = c.currentTempPath + "pack.bat";

            c.projPath = "";
            if (!string.IsNullOrEmpty(DocPath))
                c.projPath = Path.GetDirectoryName(DocPath);

            c.projMetaPath = DocPath + ".meta";

            c.rootCode = "Include\'THlib.lua\'\nInclude\'_editor_output.lua\'";

            c.zipExePath = mainAppWithInfo.ZipExecutablePath;
            c.luaSTGExePath = mainAppWithInfo.LuaSTGExecutablePath;

            if (!mainAppWithInfo.IsEXEPathSet) throw new EXEPathNotSetException();

            c.projName = Path.GetFileNameWithoutExtension(RawDocName);

            //Find mod name
            foreach (TreeNodeBase t in TreeNodes[0].Children)
            {
                if (t is ProjSettings projs)
                {
                    if (!string.IsNullOrEmpty(projs.attributes[0].AttrInput)) c.projName = projs.NonMacrolize(0);
                    break;
                }
            }

            c.luaSTGFolder = Path.GetDirectoryName(c.luaSTGExePath);

            c.GetPacker(mainAppWithInfo);

            foreach (IDocumentWithMeta idwm in referencedDoc)
            {
                if (idwm is PlainDocumentData pdd)
                {
                    pdd.GatherCompileInfo(mainAppWithInfo);
                    c.fileProcess.Add(pdd.CompileProcess as PartialProjectProcess);
                    //MessageBox.Show(pdd.CompileProcess.GetType().ToString());
                }
                else if (idwm is VirtualDoc vd)
                {
                    string s = vd.DocPath;
                    //try
                    {
                        DocumentData newDoc = GetNewByExtension(Path.GetExtension(s), -1
                            , Path.GetFileNameWithoutExtension(s), s, true);
                        TreeNodeBase t = newDoc.CreateNodeFromFile(s);
                        newDoc.TreeNodes.Add(t);
                        (newDoc as PlainDocumentData).parentProj = this;
                        newDoc.GatherCompileInfo(mainAppWithInfo);
                        c.fileProcess.Add(newDoc.CompileProcess as PartialProjectProcess);
                        //MessageBox.Show(newDoc.CompileProcess.GetType().ToString());
                    }
                    //catch { }
                }
            }

        }
    }
}
