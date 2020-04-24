using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Node.General;
using LuaSTGEditorSharp.EditorData.Node.Data;
using LuaSTGEditorSharp.EditorData.Node.Stage;
using LuaSTGEditorSharp.EditorData.Node.Task;
using LuaSTGEditorSharp.EditorData.Node.Enemy;
using LuaSTGEditorSharp.EditorData.Node.Boss;
using LuaSTGEditorSharp.EditorData.Node.Bullet;
using LuaSTGEditorSharp.EditorData.Node.Laser;
using LuaSTGEditorSharp.EditorData.Node.Object;
using LuaSTGEditorSharp.EditorData.Node.Graphics;
using LuaSTGEditorSharp.EditorData.Node.Audio;
using LuaSTGEditorSharp.EditorData.Node.Render;

namespace LuaSTGEditorSharp
{
    public class PluginToolbox : AbstractToolbox
    {
        public PluginToolbox(IMainWindow mw) : base(mw) { }
        
        public override void InitFunc()
        {
            ToolInfo["Advanced"].Add(new ToolboxItemData(true), null);
            ToolInfo["Advanced"].Add(new ToolboxItemData("taskrepeatadv", "/LuaSTGPlusNodeLib;component/images/taskadvancedrepeat.png", "Task Advanced Repeat")
                , new AddNode(AddAdvancedRepeatWithWait));

            var data = new Dictionary<ToolboxItemData, AddNode>();
            #region data
            data.Add(new ToolboxItemData("localvar", "/LuaSTGPlusNodeLib;component/images/variable.png", "Local Variable")
                , new AddNode(AddLocalVarNode));
            data.Add(new ToolboxItemData("assign", "/LuaSTGPlusNodeLib;component/images/assignment.png", "Assignment")
                , new AddNode(AddAssignmentNode));
            data.Add(new ToolboxItemData("function", "/LuaSTGPlusNodeLib;component/images/func.png", "Function")
                , new AddNode(AddFunctionNode));
            data.Add(new ToolboxItemData(true), null);
            data.Add(new ToolboxItemData("recordpos", "/LuaSTGPlusNodeLib;component/images/positionVar.png", "Record Position")
                ,  new AddNode(AddRecordPosNode));
            data.Add(new ToolboxItemData("assignpos", "/LuaSTGPlusNodeLib;component/images/positionassignment.png", "Position Assignment")
                , new AddNode(AddPositionAssignmentNode));
            #endregion
            ToolInfo.Add("Data", data);

            var stage = new Dictionary<ToolboxItemData, AddNode>();
            #region stage
            stage.Add(new ToolboxItemData("stagegroup", "/LuaSTGPlusNodeLib;component/images/stagegroup.png", "Stage Group")
                , new AddNode(AddStageGroupNode));
            stage.Add(new ToolboxItemData("stage", "/LuaSTGPlusNodeLib;component/images/stage.png", "Stage")
                , new AddNode(AddStageNode));
            stage.Add(new ToolboxItemData(true), null);
            stage.Add(new ToolboxItemData("stagegoto", "/LuaSTGPlusNodeLib;component/images/stagegoto.png", "Go to Stage")
                , new AddNode(AddStageGoToNode));
            stage.Add(new ToolboxItemData("stagegroupfinish", "/LuaSTGPlusNodeLib;component/images/stagefinishgroup.png", "Finish Stage Group")
                , new AddNode(AddStageGroupFinishNode));
            stage.Add(new ToolboxItemData(true), null);
            stage.Add(new ToolboxItemData("setstagebg", "/LuaSTGPlusNodeLib;component/images/bgstage.png", "Set Stage Background")
                , new AddNode(AddSetStageBGNode));
            stage.Add(new ToolboxItemData(true), null);
            stage.Add(new ToolboxItemData("shakescreen", "/LuaSTGPlusNodeLib;component/images/shakescreen.png", "Shake Screen")
                , new AddNode(AddShakeScreenNode));
            #endregion
            ToolInfo.Add("Stage", stage);

            var task = new Dictionary<ToolboxItemData, AddNode>();
            #region task
            task.Add(new ToolboxItemData("task", "/LuaSTGPlusNodeLib;component/images/task.png", "Task")
                , new AddNode(AddTaskNode));
            task.Add(new ToolboxItemData("tasker", "/LuaSTGPlusNodeLib;component/images/tasker.png", "Tasker")
                , new AddNode(AddTaskerNode));
            task.Add(new ToolboxItemData("taskdefine", "/LuaSTGPlusNodeLib;component/images/taskdefine.png", "Define Task")
                , new AddNode(AddTaskDefineNode));
            task.Add(new ToolboxItemData("taskcreate", "/LuaSTGPlusNodeLib;component/images/taskattach.png", "Create Task")
                , new AddNode(AddTaskCreateNode));
            task.Add(new ToolboxItemData("taskfinish", "/LuaSTGPlusNodeLib;component/images/taskreturn.png", "Finish Task")
                , new AddNode(AddTaskFinishNode));
            task.Add(new ToolboxItemData("taskclear", "/LuaSTGPlusNodeLib;component/images/taskclear.png", "Clear Task")
                , new AddNode(AddTaskClearNode));
            task.Add(new ToolboxItemData(true), null);
            task.Add(new ToolboxItemData("wait", "/LuaSTGPlusNodeLib;component/images/taskwait.png", "Wait")
                , new AddNode(AddTaskWaitNode));
            task.Add(new ToolboxItemData("taskrepeat", "/LuaSTGPlusNodeLib;component/images/taskrepeat.png", "Task Repeat")
                , new AddNode(AddTaskRepeatNode));
            task.Add(new ToolboxItemData(true), null);
            task.Add(new ToolboxItemData("moveto", "/LuaSTGPlusNodeLib;component/images/taskmoveto.png", "Move To")
                , new AddNode(AddTaskMoveToNode));
            task.Add(new ToolboxItemData("moveby", "/LuaSTGPlusNodeLib;component/images/taskmovetoex.png", "Move By")
                , new AddNode(AddTaskMoveByNode));
            task.Add(new ToolboxItemData("movetocurve", "/LuaSTGPlusNodeLib;component/images/taskBeziermoveto.png", "Move To (Curve)")
                , new AddNode(AddTaskMoveToCurveNode));
            task.Add(new ToolboxItemData("movebycurve", "/LuaSTGPlusNodeLib;component/images/taskBeziermovetoex.png", "Move By (Curve)")
                , new AddNode(AddTaskMoveByCurveNode));
            task.Add(new ToolboxItemData(true), null);
            task.Add(new ToolboxItemData("smoothset", "/LuaSTGPlusNodeLib;component/images/tasksetvalue.png", "Smooth set value to")
                , new AddNode(AddSmoothSetValueNode));
            #endregion
            ToolInfo.Add("Task", task);
            
            var enemy = new Dictionary<ToolboxItemData, AddNode>();
            #region enemy
            enemy.Add(new ToolboxItemData("defenemy", "/LuaSTGPlusNodeLib;component/images/enemydefine.png", "Define Enemy")
                , new AddNode(AddEnemyDefineNode));
            enemy.Add(new ToolboxItemData("createenemy", "/LuaSTGPlusNodeLib;component/images/enemycreate.png", "Create Enemy")
                , new AddNode(AddEnemyCreateNode));
            enemy.Add(new ToolboxItemData(true), null);
            enemy.Add(new ToolboxItemData("enemysimple", "/LuaSTGPlusNodeLib;component/images/enemysimple.png", "Create Simple Enemy with Task")
                , new AddNode(AddCreateSimpleEnemyNode));
            enemy.Add(new ToolboxItemData(true), null);
            enemy.Add(new ToolboxItemData("enemycharge", "/LuaSTGPlusNodeLib;component/images/pactrometer.png", "Enemy Charge")
                , new AddNode(AddEnemyChargeNode));
            enemy.Add(new ToolboxItemData("enemywander", "/LuaSTGPlusNodeLib;component/images/taskbosswander.png", "Enemy Wander")
                , new AddNode(AddEnemyWanderNode));
            #endregion
            ToolInfo.Add("Enemy", enemy);

            var boss = new Dictionary<ToolboxItemData, AddNode>();
            #region boss
            boss.Add(new ToolboxItemData("defboss", "/LuaSTGPlusNodeLib;component/images/bossdefine.png", "Define Boss")
                , new AddNode(AddDefineBossNode));
            boss.Add(new ToolboxItemData("bosssc", "/LuaSTGPlusNodeLib;component/images/bossspellcard.png", "Define Spell Card")
                , new AddNode(AddBossSCNode));
            boss.Add(new ToolboxItemData(true), null);
            boss.Add(new ToolboxItemData("createboss", "/LuaSTGPlusNodeLib;component/images/bosscreate.png", "Create Boss")
                , new AddNode(AddCreateBossNode));
            boss.Add(new ToolboxItemData(true), null);
            boss.Add(new ToolboxItemData("bosssetwisys", "/LuaSTGPlusNodeLib;component/images/bosswalkimg.png", "Set Walk Image of an Object")
                , new AddNode(AddSetBossWISysNode));
            boss.Add(new ToolboxItemData(true), null);
            boss.Add(new ToolboxItemData("bossmoveto", "/LuaSTGPlusNodeLib;component/images/bossmoveto.png", "Boss Move To")
                , new AddNode(AddBossMoveToNode));
            boss.Add(new ToolboxItemData("dialog", "/LuaSTGPlusNodeLib;component/images/dialog.png", "Dialog")
                , new AddNode(AddDialogNode));
            boss.Add(new ToolboxItemData("sentence", "/LuaSTGPlusNodeLib;component/images/sentence.png", "Sentence")
                , new AddNode(AddSentenceNode));
            boss.Add(new ToolboxItemData(true), null);
            boss.Add(new ToolboxItemData("bosscast", "/LuaSTGPlusNodeLib;component/images/bosscast.png", "Play cast animation")
                , new AddNode(AddBossCastNode));
            boss.Add(new ToolboxItemData("bossexplode", "/LuaSTGPlusNodeLib;component/images/bossexplode.png", "Boss Explode")
                , new AddNode(AddBossExplodeNode));
            boss.Add(new ToolboxItemData("bossaura", "/LuaSTGPlusNodeLib;component/images/bossshowaura.png", "Boss Aura")
                , new AddNode(AddBossAuraNode));
            boss.Add(new ToolboxItemData("bossui", "/LuaSTGPlusNodeLib;component/images/bosssetui.png", "Set Boss UI")
                , new AddNode(AddBossUINode));
            boss.Add(new ToolboxItemData(true), null);
            boss.Add(new ToolboxItemData("defbossbg", "/LuaSTGPlusNodeLib;component/images/bgdefine.png", "Define Boss Background")
                , new AddNode(AddBossBGDefineNode));
            boss.Add(new ToolboxItemData("bossbglayer", "/LuaSTGPlusNodeLib;component/images/bglayer.png", "Define Boss Background Layer")
                , new AddNode(AddBossBGLayerNode));
            #endregion
            ToolInfo.Add("Boss", boss);

            var bullet = new Dictionary<ToolboxItemData, AddNode>();
            #region bullet
            bullet.Add(new ToolboxItemData("defbullet", "/LuaSTGPlusNodeLib;component/images/bulletdefine.png", "Define Bullet")
                , new AddNode(AddDefineBulletNode));
            bullet.Add(new ToolboxItemData("createbullet", "/LuaSTGPlusNodeLib;component/images/bulletcreate.png", "Create Bullet")
                , new AddNode(AddCreateBulletNode));
            bullet.Add(new ToolboxItemData(true), null);
            bullet.Add(new ToolboxItemData("simplebullet", "/LuaSTGPlusNodeLib;component/images/bulletcreatestraight.png", "Create Simple Bullet")
                , new AddNode(AddCreateSimpleBulletNode));
            bullet.Add(new ToolboxItemData("bulletgroup", "/LuaSTGPlusNodeLib;component/images/bulletcreatestraightex.png", "Create Simple Bullet Group")
                , new AddNode(AddCreateBulletGroupNode));
            bullet.Add(new ToolboxItemData(true), null);
            bullet.Add(new ToolboxItemData("bulletstyle", "/LuaSTGPlusNodeLib;component/images/bulletchangestyle.png", "Change Bullet Style")
                , new AddNode(AddBulletChangeStyleNode));
            bullet.Add(new ToolboxItemData(true), null);
            bullet.Add(new ToolboxItemData("bulletclear", "/LuaSTGPlusNodeLib;component/images/bulletclear.png", "Clear Bullets")
                , new AddNode(AddBulletClearNode));
            bullet.Add(new ToolboxItemData("bulletclearrange", "/LuaSTGPlusNodeLib;component/images/bulletcleanrange.png", "Clear Bullets in range")
                , new AddNode(AddBulletClearRangeNode));
            #endregion
            ToolInfo.Add("Bullet", bullet);

            var laser = new Dictionary<ToolboxItemData, AddNode>();
            #region laser
            laser.Add(new ToolboxItemData("deflaser", "/LuaSTGPlusNodeLib;component/images/laserdefine.png", "Define Laser")
                , new AddNode(AddDefineLaserNode));
            laser.Add(new ToolboxItemData("createlaser", "/LuaSTGPlusNodeLib;component/images/lasercreate.png", "Create Laser")
                , new AddNode(AddCreateLaserNode));
            laser.Add(new ToolboxItemData(true), null);
            laser.Add(new ToolboxItemData("defbentlaser", "/LuaSTGPlusNodeLib;component/images/laserbentdefine.png", "Define Bent Laser")
                , new AddNode(AddDefineBentLaserNode));
            laser.Add(new ToolboxItemData("createbentlaser", "/LuaSTGPlusNodeLib;component/images/laserbentcreate.png", "Create Bent Laser")
                , new AddNode(AddCreateBentLaserNode));
            laser.Add(new ToolboxItemData(true), null);
            laser.Add(new ToolboxItemData("laserturnhalfon", "/LuaSTGPlusNodeLib;component/images/laserturnhalfon.png", "Turn Half On Laser")
                , new AddNode(AddLaserTurnHalfOnNode));
            laser.Add(new ToolboxItemData("laserturnon", "/LuaSTGPlusNodeLib;component/images/laserturnon.png", "Turn On Laser")
                , new AddNode(AddLaserTurnOnNode));
            laser.Add(new ToolboxItemData("laserturnoff", "/LuaSTGPlusNodeLib;component/images/laserturnoff.png", "Turn Off Laser")
                , new AddNode(AddLaserTurnOffNode));
            laser.Add(new ToolboxItemData(true), null);
            laser.Add(new ToolboxItemData("lasergrow", "/LuaSTGPlusNodeLib;component/images/lasergrow.png", "Grow Laser")
                , new AddNode(AddLaserGrowNode));
            laser.Add(new ToolboxItemData("laserchangestyle", "/LuaSTGPlusNodeLib;component/images/laserchangestyle.png", "Change Laser Style")
                , new AddNode(AddLaserChangeStyleNode));
            #endregion
            ToolInfo.Add("Laser", laser);

            var obj = new Dictionary<ToolboxItemData, AddNode>();
            #region object
            obj.Add(new ToolboxItemData("defobject", "/LuaSTGPlusNodeLib;component/images/objectdefine.png", "Define Object")
                , new AddNode(AddDefineObjectNode));
            obj.Add(new ToolboxItemData("createobject", "/LuaSTGPlusNodeLib;component/images/objectcreate.png", "Create Object")
                , new AddNode(AddCreateObjectNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("callbackfunc", "/LuaSTGPlusNodeLib;component/images/callbackfunc.png", "Call Back Functions")
                , new AddNode(AddCallBackFuncNode));
            obj.Add(new ToolboxItemData("defaultaction", "/LuaSTGPlusNodeLib;component/images/defaultaction.png", "Default Action")
                , new AddNode(AddDefaultActionNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("setv", "/LuaSTGPlusNodeLib;component/images/setv.png", "Set Velocity")
                , new AddNode(AddSetVNode));
            obj.Add(new ToolboxItemData("seta", "/LuaSTGPlusNodeLib;component/images/setaccel.png", "Set Acceleration")
                , new AddNode(AddSetANode));
            obj.Add(new ToolboxItemData("setg", "/LuaSTGPlusNodeLib;component/images/setgravity.png", "Set Gravity")
                , new AddNode(AddSetGNode));
            obj.Add(new ToolboxItemData("setvlim", "/LuaSTGPlusNodeLib;component/images/setfv.png", "Set Velocity Limit")
                , new AddNode(AddSetVLimitNode));
            obj.Add(new ToolboxItemData("delete", "/LuaSTGPlusNodeLib;component/images/unitdel.png", "Delete Unit")
                , new AddNode(AddDelNode));
            obj.Add(new ToolboxItemData("kill", "/LuaSTGPlusNodeLib;component/images/unitkill.png", "Kill Unit")
                , new AddNode(AddKillNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("setimage", "/LuaSTGPlusNodeLib;component/images/objectsetimg.png", "Set Image")
                , new AddNode(AddSetObjectImageNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("setblend", "/LuaSTGPlusNodeLib;component/images/setcolor.png", "Set Color and Blend Mode")
                , new AddNode(AddSetBlendNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("setbinding", "/LuaSTGPlusNodeLib;component/images/connect.png", "Set Parent")
                , new AddNode(AddSetBindingNode));
            obj.Add(new ToolboxItemData("setrelativepos", "/LuaSTGPlusNodeLib;component/images/setrelpos.png", "Set Relative Position")
                , new AddNode(AddSetRelativePositionNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("smear", "/LuaSTGPlusNodeLib;component/images/smear.png", "Create Smear")
                , new AddNode(AddMakeSmearNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("dropitem", "/LuaSTGPlusNodeLib;component/images/dropitem.png", "Drop Item")
                , new AddNode(AddDropItemNode));
            obj.Add(new ToolboxItemData(true), null);
            obj.Add(new ToolboxItemData("groupforeach", "/LuaSTGPlusNodeLib;component/images/unitforeach.png", "For Each Unit in Group")
                , new AddNode(AddGroupForEachNode));
            #endregion
            ToolInfo.Add("Object", obj);

            var graphics = new Dictionary<ToolboxItemData, AddNode>();
            #region graphics
            graphics.Add(new ToolboxItemData("loadimage", "/LuaSTGPlusNodeLib;component/images/loadimage.png", "Load Image")
                , new AddNode(AddLoadImageNode));
            graphics.Add(new ToolboxItemData("loadimagegroup", "/LuaSTGPlusNodeLib;component/images/loadimagegroup.png", "Load Image Group")
                , new AddNode(AddLoadImageGroupNode));
            graphics.Add(new ToolboxItemData("loadparticle", "/LuaSTGPlusNodeLib;component/images/loadparticle.png", "Load Particle")
                , new AddNode(AddLoadParticleNode));
            graphics.Add(new ToolboxItemData("loadani", "/LuaSTGPlusNodeLib;component/images/loadani.png", "Load Animation")
                , new AddNode(AddLoadAnimationNode));
            graphics.Add(new ToolboxItemData("loadfx", "/LuaSTGPlusNodeLib;component/images/loadFX.png", "Load FX")
                , new AddNode(AddLoadFXNode));
            #endregion
            ToolInfo.Add("Graphics", graphics);

            var audio = new Dictionary<ToolboxItemData, AddNode>();
            #region audio
            audio.Add(new ToolboxItemData("loadse", "/LuaSTGPlusNodeLib;component/images/loadsound.png", "Load Sound Effect")
                , new AddNode(AddLoadSENode));
            audio.Add(new ToolboxItemData("playse", "/LuaSTGPlusNodeLib;component/images/playsound.png", "Play Sound Effect")
                , new AddNode(AddPlaySENode));
            audio.Add(new ToolboxItemData(true), null);
            audio.Add(new ToolboxItemData("loadbgm", "/LuaSTGPlusNodeLib;component/images/loadbgm.png", "Load Background Music")
                , new AddNode(AddLoadBGMNode));
            audio.Add(new ToolboxItemData("playbgm", "/LuaSTGPlusNodeLib;component/images/playbgm.png", "Play Background Music")
                , new AddNode(AddPlayBGMNode));
            audio.Add(new ToolboxItemData("pausebgm", "/LuaSTGPlusNodeLib;component/images/pausebgm.png", "Pause Background Music")
                , new AddNode(AddPauseBGMNode));
            audio.Add(new ToolboxItemData("resumebgm", "/LuaSTGPlusNodeLib;component/images/resumebgm.png", "Resume Background Music")
                , new AddNode(AddResumeBGMNode));
            audio.Add(new ToolboxItemData("stopbgm", "/LuaSTGPlusNodeLib;component/images/stopbgm.png", "Stop Background Music")
                , new AddNode(AddStopBGMNode));
            #endregion
            ToolInfo.Add("Audio", audio);

            var inherit = new Dictionary<ToolboxItemData, AddNode>();
            ToolInfo.Add("Inheritance", inherit);

            var render = new Dictionary<ToolboxItemData, AddNode>();
            #region render
            render.Add(new ToolboxItemData("onrender", "/LuaSTGPlusNodeLib;component/images/onrender.png", "On Render")
                , new AddNode(AddOnRenderNode));
            render.Add(new ToolboxItemData(true), null);
            render.Add(new ToolboxItemData("r4v", "/LuaSTGPlusNodeLib;component/images/render4v.png", "Render4V")
                , new AddNode(AddR4VNode));
            render.Add(new ToolboxItemData(true), null);
            render.Add(new ToolboxItemData("creatertar", "/LuaSTGPlusNodeLib;component/images/CreateRenderTarget.png", "Create Render Target")
                , new AddNode(AddCreateRenderTargetNode));
            render.Add(new ToolboxItemData("rtarop", "/LuaSTGPlusNodeLib;component/images/RenderTarget.png", "Push/Pop Render Target")
                , new AddNode(AddRenderTargetNode));
            //render.Add(new ToolboxItemData("cap", "/LuaSTGPlusNodeLib;component/images/PostEffectCapture.png", "Begin Texture Capturing")
            //    , new AddNode(AddPostEffectCaptureNode));
            render.Add(new ToolboxItemData("posteff", "/LuaSTGPlusNodeLib;component/images/PostEffect.png", "Post Effect")
                , new AddNode(AddPostEffectNode));
            #endregion
            ToolInfo.Add("Render", render);

            var background = new Dictionary<ToolboxItemData, AddNode>();
            ToolInfo.Add("Background", background);

            var player = new Dictionary<ToolboxItemData, AddNode>();
            ToolInfo.Add("Player", player);
        }
        
        #region data
        private void AddLocalVarNode()
        {
            parent.Insert(new LocalVar(parent.ActivatedWorkSpaceData));
        }

        private void AddAssignmentNode()
        {
            parent.Insert(new Assignment(parent.ActivatedWorkSpaceData));
        }
        private void AddFunctionNode()
        {
            parent.Insert(new Function(parent.ActivatedWorkSpaceData));
        }

        private void AddRecordPosNode()
        {
            parent.Insert(new RecordPos(parent.ActivatedWorkSpaceData));
        }

        private void AddPositionAssignmentNode()
        {
            parent.Insert(new PositionAssignment(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region stage
        private void AddStageGroupNode()
        {
            TreeNode newStG = new StageGroup(parent.ActivatedWorkSpaceData);
                TreeNode newSt = new Stage(parent.ActivatedWorkSpaceData);
                    TreeNode newTask = new TaskNode(parent.ActivatedWorkSpaceData);
                        TreeNode newFolder = new Folder(parent.ActivatedWorkSpaceData, "Initialize");
                            newFolder.AddChild(new StageBG(parent.ActivatedWorkSpaceData));
                        newTask.AddChild(newFolder);
                        newTask.AddChild(new TaskWait(parent.ActivatedWorkSpaceData, "240"));
                    newSt.AddChild(newTask);
                newStG.AddChild(newSt);
            parent.Insert(newStG);
        }

        private void AddStageNode()
        {
            TreeNode newSt = new Stage(parent.ActivatedWorkSpaceData);
                TreeNode newTask = new TaskNode(parent.ActivatedWorkSpaceData);
                    TreeNode newFolder = new Folder(parent.ActivatedWorkSpaceData, "Initialize");
                        newFolder.AddChild(new StageBG(parent.ActivatedWorkSpaceData));
                    newTask.AddChild(newFolder);
                    newTask.AddChild(new TaskWait(parent.ActivatedWorkSpaceData, "240"));
                newSt.AddChild(newTask);
            parent.Insert(newSt);
        }

        private void AddSetStageBGNode()
        {
            TreeNode newStBG = new StageBG(parent.ActivatedWorkSpaceData);
            parent.Insert(newStBG);
        }

        private void AddShakeScreenNode()
        {
            parent.Insert(new ShakeScreen(parent.ActivatedWorkSpaceData));
        }

        private void AddStageGoToNode()
        {
            parent.Insert(new StageGoto(parent.ActivatedWorkSpaceData));
        }

        private void AddStageGroupFinishNode()
        {
            parent.Insert(new StageGroupFinish(parent.ActivatedWorkSpaceData));
        }

        #endregion
        #region task
        private void AddTaskNode()
        {
            parent.Insert(new TaskNode(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskerNode()
        {
            parent.Insert(new Tasker(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskDefineNode()
        {
            parent.Insert(new TaskDefine(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskCreateNode()
        {
            parent.Insert(new TaskCreate(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskFinishNode()
        {
            parent.Insert(new TaskFinish(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskClearNode()
        {
            parent.Insert(new TaskClear(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskWaitNode()
        {
            parent.Insert(new TaskWait(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskRepeatNode()
        {
            TreeNode repeat = new Repeat(parent.ActivatedWorkSpaceData, "_infinite");
            repeat.AddChild(new TaskWait(parent.ActivatedWorkSpaceData));
            parent.Insert(repeat);
        }

        private void AddTaskMoveToNode()
        {
            parent.Insert(new TaskMoveTo(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskMoveByNode()
        {
            parent.Insert(new TaskMoveBy(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskMoveToCurveNode()
        {
            parent.Insert(new TaskMoveToCurve(parent.ActivatedWorkSpaceData));
        }

        private void AddTaskMoveByCurveNode()
        {
            parent.Insert(new TaskMoveByCurve(parent.ActivatedWorkSpaceData));
        }

        private void AddSmoothSetValueNode()
        {
            parent.Insert(new SmoothSetValueTo(parent.ActivatedWorkSpaceData));
        }
        #endregion

        private void AddAdvancedRepeatWithWait()
        {
            TreeNode repeat = new EditorData.Node.Advanced.AdvancedRepeat.AdvancedRepeat(parent.ActivatedWorkSpaceData, "_infinite");
            repeat.AddChild(new EditorData.Node.Advanced.AdvancedRepeat.VariableCollection(parent.ActivatedWorkSpaceData));
            repeat.AddChild(new TaskWait(parent.ActivatedWorkSpaceData));
            parent.Insert(repeat);
        }

        #region enemy
        private void AddEnemyDefineNode()
        {
            TreeNode newDef = new EnemyDefine(parent.ActivatedWorkSpaceData);
            newDef.AddChild(new EnemyInit(parent.ActivatedWorkSpaceData));
            parent.Insert(newDef);
        }

        private void AddEnemyCreateNode()
        {
            parent.Insert(new CreateEnemy(parent.ActivatedWorkSpaceData));
        }

        private void AddCreateSimpleEnemyNode()
        {
            parent.Insert(new CreateSimpleEnemy(parent.ActivatedWorkSpaceData));
        }

        private void AddEnemyChargeNode()
        {
            parent.Insert(new EnemyCharge(parent.ActivatedWorkSpaceData));
        }

        private void AddEnemyWanderNode()
        {
            parent.Insert(new EnemyWander(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region boss
        private void AddDefineBossNode()
        {
            TreeNode newDef = new BossDefine(parent.ActivatedWorkSpaceData);
            TreeNode init = new BossInit(parent.ActivatedWorkSpaceData);
            TreeNode newSC = new BossSpellCard(parent.ActivatedWorkSpaceData);
            TreeNode newSCStart = new BossSCStart(parent.ActivatedWorkSpaceData);
            TreeNode newTask = new TaskNode(parent.ActivatedWorkSpaceData);
            newSCStart.AddChild(newTask);
            newTask.AddChild(new TaskMoveTo(parent.ActivatedWorkSpaceData, "0,120", "60", "MOVE_NORMAL"));
            newSC.AddChild(newSCStart);
            newSC.AddChild(new BossSCFinish(parent.ActivatedWorkSpaceData));
            newDef.AddChild(init);
            newDef.AddChild(newSC);
            parent.Insert(newDef);
        }

        private void AddBossSCNode()
        {
            TreeNode newSC = new BossSpellCard(parent.ActivatedWorkSpaceData);
            TreeNode newSCStart = new BossSCStart(parent.ActivatedWorkSpaceData);
            TreeNode newTask = new TaskNode(parent.ActivatedWorkSpaceData);
            newSCStart.AddChild(newTask);
            newTask.AddChild(new TaskMoveTo(parent.ActivatedWorkSpaceData, "0,120", "60", "MOVE_NORMAL"));
            newSC.AddChild(newSCStart);
            newSC.AddChild(new BossSCFinish(parent.ActivatedWorkSpaceData));
            parent.Insert(newSC);
        }

        private void AddCreateBossNode()
        {
            parent.Insert(new CreateBoss(parent.ActivatedWorkSpaceData));
        }

        private void AddBossMoveToNode()
        {
            parent.Insert(new BossMoveTo(parent.ActivatedWorkSpaceData));
        }

        private void AddDialogNode()
        {
            TreeNode dialog = new Dialog(parent.ActivatedWorkSpaceData);
            dialog.AddChild(new TaskNode(parent.ActivatedWorkSpaceData));
            parent.Insert(dialog);
        }

        private void AddSentenceNode()
        {
            parent.Insert(new Sentence(parent.ActivatedWorkSpaceData));
        }

        private void AddSetBossWISysNode()
        {
            parent.Insert(new SetBossWalkImageSystem(parent.ActivatedWorkSpaceData));
        }

        private void AddBossCastNode()
        {
            parent.Insert(new BossCast(parent.ActivatedWorkSpaceData));
        }

        private void AddBossExplodeNode()
        {
            parent.Insert(new BossExplode(parent.ActivatedWorkSpaceData));
        }

        private void AddBossAuraNode()
        {
            parent.Insert(new BossAura(parent.ActivatedWorkSpaceData));
        }

        private void AddBossUINode()
        {
            parent.Insert(new BossUI(parent.ActivatedWorkSpaceData));
        }

        private void AddBossBGDefineNode()
        {
            parent.Insert(new BossBGDefine(parent.ActivatedWorkSpaceData));
        }

        private void AddBossBGLayerNode()
        {
            TreeNode newDef = new BossBGLayer(parent.ActivatedWorkSpaceData);
            newDef.AddChild(new BossBGLayerInit(parent.ActivatedWorkSpaceData));
            newDef.AddChild(new BossBGLayerFrame(parent.ActivatedWorkSpaceData));
            newDef.AddChild(new BossBGLayerRender(parent.ActivatedWorkSpaceData));
            parent.Insert(newDef);
        }
        #endregion
        #region bullet
        private void AddDefineBulletNode()
        {
            TreeNode newDef = new BulletDefine(parent.ActivatedWorkSpaceData);
            newDef.AddChild(new BulletInit(parent.ActivatedWorkSpaceData));
            parent.Insert(newDef);
        }

        private void AddCreateBulletNode()
        {
            parent.Insert(new CreateBullet(parent.ActivatedWorkSpaceData));
        }

        private void AddCreateSimpleBulletNode()
        {
            parent.Insert(new CreateSimpleBullet(parent.ActivatedWorkSpaceData));
        }

        private void AddCreateBulletGroupNode()
        {
            parent.Insert(new CreateBulletGroup(parent.ActivatedWorkSpaceData));
        }

        private void AddBulletChangeStyleNode()
        {
            parent.Insert(new BulletChangeStyle(parent.ActivatedWorkSpaceData));
        }

        private void AddBulletClearNode()
        {
            parent.Insert(new BulletClear(parent.ActivatedWorkSpaceData));
        }

        private void AddBulletClearRangeNode()
        {
            parent.Insert(new BulletClearRange(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region laser
        private void AddDefineLaserNode()
        {
            TreeNode newDef = new LaserDefine(parent.ActivatedWorkSpaceData);
            TreeNode newInit = new LaserInit(parent.ActivatedWorkSpaceData);
            TreeNode newTask = new TaskNode(parent.ActivatedWorkSpaceData);
            TreeNode newTurnOn = new LaserTurnOn(parent.ActivatedWorkSpaceData);
            newTask.AddChild(newTurnOn);
            newInit.AddChild(newTask);
            newDef.AddChild(newInit);
            parent.Insert(newDef);
        }

        private void AddCreateLaserNode()
        {
            parent.Insert(new CreateLaser(parent.ActivatedWorkSpaceData));
        }

        private void AddDefineBentLaserNode()
        {
            TreeNode newDef = new BentLaserDefine(parent.ActivatedWorkSpaceData);
            newDef.AddChild(new BentLaserInit(parent.ActivatedWorkSpaceData));
            parent.Insert(newDef);
        }

        private void AddCreateBentLaserNode()
        {
            parent.Insert(new CreateBentLaser(parent.ActivatedWorkSpaceData));
        }

        private void AddLaserTurnHalfOnNode()
        {
            parent.Insert(new LaserTurnHalfOn(parent.ActivatedWorkSpaceData));
        }

        private void AddLaserTurnOnNode()
        {
            parent.Insert(new LaserTurnOn(parent.ActivatedWorkSpaceData));
        }

        private void AddLaserTurnOffNode()
        {
            parent.Insert(new LaserTurnOff(parent.ActivatedWorkSpaceData));
        }

        private void AddLaserGrowNode()
        {
            parent.Insert(new LaserGrow(parent.ActivatedWorkSpaceData));
        }

        private void AddLaserChangeStyleNode()
        {
            parent.Insert(new LaserChangeStyle(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region object
        private void AddDefineObjectNode()
        {
            TreeNode objdef = new ObjectDefine(parent.ActivatedWorkSpaceData);
            objdef.AddChild(new ObjectInit(parent.ActivatedWorkSpaceData));
            parent.Insert(objdef);
        }

        private void AddCreateObjectNode()
        {
            parent.Insert(new CreateObject(parent.ActivatedWorkSpaceData));
        }

        private void AddCallBackFuncNode()
        {
            TreeNode newCBF = new CallBackFunc(parent.ActivatedWorkSpaceData);
            newCBF.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData));
            parent.Insert(newCBF);
        }

        private void AddDefaultActionNode()
        {
            parent.Insert(new DefaultAction(parent.ActivatedWorkSpaceData));
        }

        private void AddSetVNode()
        {
            parent.Insert(new SetVelocity(parent.ActivatedWorkSpaceData));
        }

        private void AddSetANode()
        {
            parent.Insert(new SetAccel(parent.ActivatedWorkSpaceData));
        }

        private void AddSetGNode()
        {
            parent.Insert(new SetGravity(parent.ActivatedWorkSpaceData));
        }

        private void AddSetVLimitNode()
        {
            parent.Insert(new SetVelocityLimit(parent.ActivatedWorkSpaceData));
        }

        private void AddDelNode()
        {
            parent.Insert(new Del(parent.ActivatedWorkSpaceData));
        }

        private void AddKillNode()
        {
            parent.Insert(new Kill(parent.ActivatedWorkSpaceData));
        }

        private void AddSetObjectImageNode()
        {
            parent.Insert(new SetObjectImage(parent.ActivatedWorkSpaceData));
        }

        private void AddSetBlendNode()
        {
            parent.Insert(new SetBlend(parent.ActivatedWorkSpaceData));
        }

        private void AddSetBindingNode()
        {
            parent.Insert(new SetBinding(parent.ActivatedWorkSpaceData));
        }

        private void AddSetRelativePositionNode()
        {
            parent.Insert(new SetRelativePosition(parent.ActivatedWorkSpaceData));
        }

        private void AddDropItemNode()
        {
            parent.Insert(new DropItem(parent.ActivatedWorkSpaceData));
        }

        private void AddMakeSmearNode()
        {
            parent.Insert(new MakeSmear(parent.ActivatedWorkSpaceData));
        }

        private void AddGroupForEachNode()
        {
            parent.Insert(new GroupForEach(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region graphics
        private void AddLoadImageNode()
        {
            parent.Insert(new LoadImage(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadImageGroupNode()
        {
            parent.Insert(new LoadImageGroup(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadParticleNode()
        {
            parent.Insert(new LoadParticle(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadAnimationNode()
        {
            parent.Insert(new LoadAnimation(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadFXNode()
        {
            parent.Insert(new LoadFX(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region audio
        private void AddLoadSENode()
        {
            parent.Insert(new LoadSE(parent.ActivatedWorkSpaceData));
        }

        private void AddPlaySENode()
        {
            parent.Insert(new PlaySE(parent.ActivatedWorkSpaceData));
        }

        private void AddLoadBGMNode()
        {
            parent.Insert(new LoadBGM(parent.ActivatedWorkSpaceData));
        }

        private void AddPlayBGMNode()
        {
            parent.Insert(new PlayBGM(parent.ActivatedWorkSpaceData));
        }

        private void AddPauseBGMNode()
        {
            parent.Insert(new PauseBGM(parent.ActivatedWorkSpaceData));
        }

        private void AddResumeBGMNode()
        {
            parent.Insert(new ResumeBGM(parent.ActivatedWorkSpaceData));
        }

        private void AddStopBGMNode()
        {
            parent.Insert(new StopBGM(parent.ActivatedWorkSpaceData));
        }
        #endregion
        #region render
        private void AddOnRenderNode()
        {
            var o = new OnRender(parent.ActivatedWorkSpaceData);
            o.AddChild(new DefaultAction(parent.ActivatedWorkSpaceData));
            parent.Insert(o);
        }

        private void AddR4VNode()
        {
            parent.Insert(new Render4V(parent.ActivatedWorkSpaceData));
        }

        private void AddCreateRenderTargetNode()
        {
            parent.Insert(new CreateRenderTarget(parent.ActivatedWorkSpaceData));
        }

        private void AddRenderTargetNode()
        {
            parent.Insert(new RenderTarget(parent.ActivatedWorkSpaceData));
        }

        private void AddPostEffectCaptureNode()
        {
            parent.Insert(new PostEffectCapture(parent.ActivatedWorkSpaceData));
        }

        private void AddPostEffectNode()
        {
            parent.Insert(new PostEffect(parent.ActivatedWorkSpaceData));
        }
        #endregion
    }
}
