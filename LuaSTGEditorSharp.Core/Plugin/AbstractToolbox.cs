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

        public List<SearchModel> nodeNameList;

        protected readonly IMainWindow parent;

        public ObservableCollection<ToolboxTab> ToolboxTabs = new ObservableCollection<ToolboxTab>();

        public AbstractToolbox(IMainWindow mw)
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
            gen.Add(new ToolboxItemData("folder", "/LuaSTGEditorSharp.Core;component/images/folder.png", "Folder"), new AddNode(AddFolderNode));
            gen.Add(new ToolboxItemData("code", "/LuaSTGEditorSharp.Core;component/images/code.png", "Code"), new AddNode(AddCodeNode));
            gen.Add(new ToolboxItemData("codeseg", "/LuaSTGEditorSharp.Core;component/images/codesegment.png", "Code Segment"), new AddNode(AddBlockCodeNode));
            gen.Add(new ToolboxItemData("comment", "/LuaSTGEditorSharp.Core;component/images/comment.png", "Comment"), new AddNode(AddCommentNode));
            gen.Add(new ToolboxItemData(true), null);
            gen.Add(new ToolboxItemData("if", "/LuaSTGEditorSharp.Core;component/images/if.png", "If"), new AddNode(AddIfNode));
            gen.Add(new ToolboxItemData("elseif", "/LuaSTGEditorSharp.Core;component/images/elseif.png", "Else If"), new AddNode(AddElseIfNode));
            gen.Add(new ToolboxItemData("repeat", "/LuaSTGEditorSharp.Core;component/images/repeat.png", "Repeat"), new AddNode(AddRepeatNode));
            gen.Add(new ToolboxItemData("break", "/LuaSTGEditorSharp.Core;component/images/break.png", "Break"), new AddNode(AddBreakNode));
            gen.Add(new ToolboxItemData("codeblock", "/LuaSTGEditorSharp.Core;component/images/codeblock.png", "Code Block"), new AddNode(AddCodeBlockNode));
            gen.Add(new ToolboxItemData(true), null);
            gen.Add(new ToolboxItemData("patch", "/LuaSTGEditorSharp.Core;component/images/patch.png", "Patch"), new AddNode(AddPatchNode));
            gen.Add(new ToolboxItemData("file", "/LuaSTGEditorSharp.Core;component/images/pack.png", "Add File"), new AddNode(AddAddFileNode));
            gen.Add(new ToolboxItemData(true), null);
            gen.Add(new ToolboxItemData("region", "/LuaSTGEditorSharp.Core;component/images/region.png", "Region"), new AddNode(AddRegionNode));
            gen.Add(new ToolboxItemData(true), null);
            gen.Add(new ToolboxItemData("defmacro", "/LuaSTGEditorSharp.Core;component/images/definemacro.png", "Define Macro"), new AddNode(AddDefineMacroNode));
            gen.Add(new ToolboxItemData("archisp", "/LuaSTGEditorSharp.Core;component/images/archispace.png", "Archive Space"), new AddNode(AddArchiveSpaceNode));
            gen.Add(new ToolboxItemData(true), null);
            gen.Add(new ToolboxItemData("userdef", "/LuaSTGEditorSharp.Core;component/images/userdefinednode.png", "User Defined Node"), new AddNode(AddUserDefinedNode));
            gen.Add(new ToolboxItemData("unidentified", "/LuaSTGEditorSharp.Core;component/images/unidentifiednode.png", "Unidentified Node"), new AddNode(AddUnidentifiedNode));
            #endregion
            ToolInfo.Add("General", gen);

            var ar = new Dictionary<ToolboxItemData, AddNode>();
            #region advanced
            ar.Add(new ToolboxItemData("advrepeat", "/LuaSTGEditorSharp.Core;component/images/advancedrepeat.png", "Advanced Repeat"), new AddNode(AddAdvancedRepeatNode));
            ar.Add(new ToolboxItemData(true), null);
            ar.Add(new ToolboxItemData("rvarincrement", "/LuaSTGEditorSharp.Core;component/images/LinearVariable.png", "Increment Variable"), new AddNode(AddIncrementVariableNode));
            ar.Add(new ToolboxItemData(true), null);
            ar.Add(new ToolboxItemData("rvarlinear", "/LuaSTGEditorSharp.Core;component/images/LinearVariable.png", "Linear Variable"), new AddNode(AddLinearVariableNode));
            ar.Add(new ToolboxItemData("rvarsineinterp", "/LuaSTGEditorSharp.Core;component/images/SinusoidalInterpolationVariable.png", "Sinusoidal Interpolation Variable")
                , new AddNode(AddSinusoidalInterpolationVariableNode));
            ar.Add(new ToolboxItemData("rvarsinemov", "/LuaSTGEditorSharp.Core;component/images/SinusoidalMovementVariable.png", "Sinusoidal Movement Variable")
                , new AddNode(AddSinusoidalMovementVariableNode));
            ar.Add(new ToolboxItemData(true), null);
            ar.Add(new ToolboxItemData("rvarrebound", "/LuaSTGEditorSharp.Core;component/images/ReboundingVariable.png", "Rebounding Variable"), new AddNode(AddReboundingVariableNode));
            ar.Add(new ToolboxItemData("rvarsineosc", "/LuaSTGEditorSharp.Core;component/images/SinusoidalOscillationVariable.png", "Sinusoidal Oscillation Variable")
                , new AddNode(AddSinusoidalOscillationVariableNode));
            #endregion
            ToolInfo.Add("Advanced", ar);
        }

        private void InitData()
        {
            var proj = new Dictionary<ToolboxItemData, AddNode>();
            #region project
            proj.Add(new ToolboxItemData("projectfile", "/LuaSTGEditorSharp.Core;component/images/patch.png", "Load as Part of Project"), new AddNode(AddProjectFileNode));
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

        private void AddAddFileNode()
        {
            parent.Insert(new AddFile(parent.ActivatedWorkSpaceData));
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

        private void AddIncrementVariableNode()
        {
            parent.Insert(new IncrementVariable(parent.ActivatedWorkSpaceData));
        }

        private void AddLinearVariableNode()
        {
            parent.Insert(new LinearVariable(parent.ActivatedWorkSpaceData));
        }

        private void AddSinusoidalInterpolationVariableNode()
        {
            parent.Insert(new SinusoidalInterpolationVariable(parent.ActivatedWorkSpaceData));
        }

        private void AddSinusoidalMovementVariableNode()
        {
            parent.Insert(new SinusoidalMovementVariable(parent.ActivatedWorkSpaceData));
        }

        private void AddReboundingVariableNode()
        {
            parent.Insert(new ReboundingVariable(parent.ActivatedWorkSpaceData));
        }

        private void AddSinusoidalOscillationVariableNode()
        {
            parent.Insert(new SinusoidalOscillationVariable(parent.ActivatedWorkSpaceData));
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
