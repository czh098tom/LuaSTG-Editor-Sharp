using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using LuaSTGEditorSharp.Toolbox;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Node.General;
using LuaSTGEditorSharp.EditorData.Node.Advanced;
using LuaSTGEditorSharp.EditorData.Node.Advanced.AdvancedRepeat;
using LuaSTGEditorSharp.EditorData.Node.Project;

namespace LuaSTGEditorSharp.Plugin
{
    public abstract class AbstractToolbox
    {
        public delegate void AddNode();

        protected Dictionary<string, Dictionary<ToolboxItemData, AddNode>> ToolInfo 
            = new Dictionary<string, Dictionary<ToolboxItemData, AddNode>>();

        public Dictionary<string, AddNode> NFuncs = new Dictionary<string, AddNode>();

        internal List<SearchModel> nodeNameList;

        protected readonly MainWindow parent;

        public ObservableCollection<ToolboxTab> ToolboxTabs = new ObservableCollection<ToolboxTab>();

        public AbstractToolbox(MainWindow mw)
        {
            parent = mw;
            InitGen();
            InitFunc();
            InitData();
        }

        public abstract void InitFunc();

        protected void InitGen()
        {
            var gen = new Dictionary<ToolboxItemData, AddNode>();
            #region general
            gen.Add(new ToolboxItemData("folder", "images/folder.png", "Folder"), new AddNode(AddFolderNode));
            gen.Add(new ToolboxItemData("code", "images/code.png", "Code"), new AddNode(AddCodeNode));
            gen.Add(new ToolboxItemData("codeseg", "images/codesegment.png", "Code Segment"), new AddNode(AddBlockCodeNode));
            gen.Add(new ToolboxItemData("comment", "images/comment.png", "Comment"), new AddNode(AddCommentNode));
            gen.Add(new ToolboxItemData(true), null);
            gen.Add(new ToolboxItemData("if", "images/if.png", "If"), new AddNode(AddIfNode));
            gen.Add(new ToolboxItemData("elseif", "images/elseif.png", "Else If"), new AddNode(AddElseIfNode));
            gen.Add(new ToolboxItemData("repeat", "images/repeat.png", "Repeat"), new AddNode(AddRepeatNode));
            gen.Add(new ToolboxItemData("break", "images/break.png", "Break"), new AddNode(AddBreakNode));
            gen.Add(new ToolboxItemData("codeblock", "images/codeblock.png", "Break"), new AddNode(AddCodeBlockNode));
            gen.Add(new ToolboxItemData(true), null);
            gen.Add(new ToolboxItemData("patch", "images/patch.png", "Patch"), new AddNode(AddPatchNode));
            #endregion
            gen.Add(new ToolboxItemData(true), null);
            #region advanced
            gen.Add(new ToolboxItemData("region", "images/region.png", "Region"), new AddNode(AddRegionNode));
            gen.Add(new ToolboxItemData(true), null);
            gen.Add(new ToolboxItemData("defmacro", "images/definemacro.png", "Define Macro"), new AddNode(AddDefineMacroNode));
            gen.Add(new ToolboxItemData("archisp", "images/archispace.png", "Archive Space"), new AddNode(AddArchiveSpaceNode));
            gen.Add(new ToolboxItemData(true), null);
            gen.Add(new ToolboxItemData("userdef", "images/userdefinednode.png", "User Defined Node"), new AddNode(AddUserDefinedNode));
            gen.Add(new ToolboxItemData("unidentified", "images/unidentifiednode.png", "Unidentified Node"), new AddNode(AddUnidentifiedNode));
            #endregion
            ToolInfo.Add("General", gen);

            var ar = new Dictionary<ToolboxItemData, AddNode>();
            ar.Add(new ToolboxItemData("advrepeat", "images/repeat.png", "Advanced Repeat"), new AddNode(AddAdvancedRepeatNode));
            ar.Add(new ToolboxItemData(true), null);
            ar.Add(new ToolboxItemData("linearvar", "images/LinearVariable.png", "Linear Variable"), new AddNode(AddLinearVariableNode));
            ToolInfo.Add("Advanced", ar);
        }

        private void InitData()
        {
            var proj = new Dictionary<ToolboxItemData, AddNode>();
            #region project
            proj.Add(new ToolboxItemData("projectfile", "images/patch.png", "Load as Part of Project"), new AddNode(AddProjectFileNode));
            #endregion
            ToolInfo.Add("Project", proj);

            ToolboxTabs = new ObservableCollection<ToolboxTab>(
                from KeyValuePair<string, Dictionary<ToolboxItemData, AddNode>> kvp
                in ToolInfo
                select new ToolboxTab()
                {
                    Header = kvp.Key,
                    Data = new ObservableCollection<ToolboxItemData>(
                    from KeyValuePair<ToolboxItemData, AddNode> kvp2
                    in kvp.Value
                    select kvp2.Key)
                });
            NFuncs = new Dictionary<string, AddNode>();
            nodeNameList = new List<SearchModel>();
            foreach (KeyValuePair<string, Dictionary<ToolboxItemData, AddNode>> kvp in ToolInfo)
            {
                foreach(KeyValuePair<ToolboxItemData,AddNode> kvp2 in kvp.Value)
                {
                    if (!kvp2.Key.IsSeperator)
                    {
                        NFuncs.Add(kvp2.Key.Tag, kvp2.Value);
                        nodeNameList.Add(new SearchModel() { Name = kvp2.Key.Tag, Icon = kvp2.Key.Image });
                    }
                }
            }
            nodeNameList.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
        }

        public IEnumerable<KeyValuePair<string,BitmapImage>> GetToolBoxImageResources()
        {
            foreach(KeyValuePair<string, Dictionary<ToolboxItemData, AddNode>> kvp in ToolInfo)
            {
                foreach(KeyValuePair<ToolboxItemData, AddNode> kvp2 in kvp.Value)
                {
                    if (!kvp2.Key.IsSeperator)
                    {
                        string s = kvp2.Key.Image;
                        yield return new KeyValuePair<string, BitmapImage>(s, new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute)));
                    }
                }
            }
        }

        #region general
        private void AddFolderNode()
        {
            parent.Insert(new Folder(parent.ActivatedWorkSpaceData));
        }

        private void AddCodeNode()
        {
            parent.Insert(new Code(parent.ActivatedWorkSpaceData));
        }

        private void AddCommentNode()
        {
            parent.Insert(new Comment(parent.ActivatedWorkSpaceData));
        }

        private void AddIfNode()
        {
            TreeNode newIf = new IfNode(parent.ActivatedWorkSpaceData);
            newIf.AddChild(new IfThen(parent.ActivatedWorkSpaceData));
            newIf.AddChild(new IfElse(parent.ActivatedWorkSpaceData));
            parent.Insert(newIf);
        }

        private void AddElseIfNode()
        {
            parent.Insert(new IfElseIf(parent.ActivatedWorkSpaceData));
        }

        private void AddRepeatNode()
        {
            parent.Insert(new Repeat(parent.ActivatedWorkSpaceData));
        }

        private void AddBreakNode()
        {
            parent.Insert(new Break(parent.ActivatedWorkSpaceData));
        }

        private void AddPatchNode()
        {
            parent.Insert(new Patch(parent.ActivatedWorkSpaceData));
        }

        private void AddCodeBlockNode()
        {
            parent.Insert(new CodeBlock(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region advanced
        private void AddDefineMacroNode()
        {
            parent.Insert(new DefineMacro(parent.ActivatedWorkSpaceData));
        }

        private void AddRegionNode()
        {
            parent.Insert(new Region(parent.ActivatedWorkSpaceData));
        }

        private void AddArchiveSpaceNode()
        {
            parent.Insert(new ArchiveSpaceIndicator(parent.ActivatedWorkSpaceData));
        }

        private void AddBlockCodeNode()
        {
            parent.Insert(new CodeSegment(parent.ActivatedWorkSpaceData));
        }

        private void AddUserDefinedNode()
        {
            parent.Insert(new UserDefinedNode(parent.ActivatedWorkSpaceData));
        }

        private void AddUnidentifiedNode()
        {
            parent.Insert(new UnidentifiedNode(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region AdvancedRepeat
        private void AddAdvancedRepeatNode()
        {
            TreeNode newAR = new AdvancedRepeat(parent.ActivatedWorkSpaceData);
            newAR.AddChild(new VariableCollection(parent.ActivatedWorkSpaceData));
            parent.Insert(newAR);
        }

        private void AddLinearVariableNode()
        {
            parent.Insert(new LinearVariable(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region project
        private void AddProjectFileNode()
        {
            parent.Insert(new ProjectFile(parent.ActivatedWorkSpaceData));
        }
        #endregion
    }
}
