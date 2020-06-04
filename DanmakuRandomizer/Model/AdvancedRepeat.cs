using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Node.Task;
using LuaSTGEditorSharp.EditorData.Node.Advanced.AdvancedRepeat;

namespace DanmakuRandomizer.Model
{
    internal class AdvancedRepeat : Node
    {
        public int Times { get; set; } = 1;
        public int Wait { get; set; } = 0;
        public List<Variable> Variables { get; set; } = new List<Variable>();
        public override string Text => $"repeat {Times} times, wait {Wait} frames";

        public override TreeNode GetTreeNode(DocumentData documentData)
        {
            var ar = new LuaSTGEditorSharp.EditorData.Node.Advanced.AdvancedRepeat.AdvancedRepeat
                (documentData, Times.ToString());
            var vc = new VariableCollection(documentData);
            foreach(Variable v in Variables)
            {
                switch (v.InterpolationType)
                {
                    case InterpolationType.Sine | InterpolationType.Accelerate:
                        vc.AddChild(new SinusoidalInterpolationVariable
                            (documentData, 
                            v.Name, 
                            v.Begin.ToString(), 
                            v.End.ToString(), 
                            "false", 
                            "MOVE_ACCEL"));
                        break;
                    case InterpolationType.Sine | InterpolationType.Decelerate:
                        vc.AddChild(new SinusoidalInterpolationVariable
                            (documentData,
                            v.Name,
                            v.Begin.ToString(),
                            v.End.ToString(),
                            "false",
                            "MOVE_DECEL"));
                        break;
                    case InterpolationType.Sine | InterpolationType.Acc_Dec:
                        vc.AddChild(new SinusoidalInterpolationVariable
                            (documentData,
                            v.Name,
                            v.Begin.ToString(),
                            v.End.ToString(),
                            "false",
                            "MOVE_ACC_DEC"));
                        break;
                    case InterpolationType.Linear:
                        vc.AddChild(new LinearVariable
                            (documentData,
                            v.Name,
                            v.Begin.ToString(),
                            v.End.ToString(),
                            "false",
                            "MOVE_NORMAL"));
                        break;
                    case InterpolationType.Linear | InterpolationType.Accelerate:
                        vc.AddChild(new LinearVariable
                            (documentData,
                            v.Name,
                            v.Begin.ToString(),
                            v.End.ToString(),
                            "false",
                            "MOVE_ACCEL"));
                        break;
                    case InterpolationType.Linear | InterpolationType.Decelerate:
                        vc.AddChild(new LinearVariable
                            (documentData,
                            v.Name,
                            v.Begin.ToString(),
                            v.End.ToString(),
                            "false",
                            "MOVE_DECEL"));
                        break;
                    case InterpolationType.Linear | InterpolationType.Acc_Dec:
                        vc.AddChild(new LinearVariable
                            (documentData,
                            v.Name,
                            v.Begin.ToString(),
                            v.End.ToString(),
                            "false",
                            "MOVE_ACC_DEC"));
                        break;
                    default:
                        break;
                }
            }
            ar.AddChild(vc);
            ar.AddChild(new TaskWait(documentData, Wait.ToString()));
            return ar;
        }
    }
}
