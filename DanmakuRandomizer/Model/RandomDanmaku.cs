using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Node.Bullet;
using LuaSTGEditorSharp.EditorData.Node.Advanced;
using LuaSTGEditorSharp.EditorData.Node.Task;
using LuaSTGEditorSharp.EditorData.Node.Boss;

namespace DanmakuRandomizer.Model
{
    internal class RandomDanmaku
    {
        List<Node> nodes;
        int seed;
        Random ran;
        int varCount;
        public int Depth { get; set; }

        internal RandomDanmaku Randomize()
        {
            seed = new Random().Next();
            ran = new Random(seed);
            varCount = 0;
            nodes = new List<Node>();
            nodes.Add(new AdvancedRepeat() { Times = 1 });
            nodes.Add(new SimpleBullet());
            for (int i = 0; i < Depth; i++)
            {
                switch (ran.Next(0, 3))
                {
                    case 0:
                        if (!AddAdvancedRepeat()) i--;
                        break;
                    case 1:
                        if (!SetVR()) i--;
                        break;
                    case 2:
                        if (!SetWait()) i--;
                        break;
                    default:
                        break;
                }
            }
            return this;
        }

        private bool AddAdvancedRepeat()
        {
            List<int> selectedID = new List<int>();
            for (int i = 1; i < nodes.Count; i++)
            {
                if (nodes[i] is SimpleBullet && nodes[i - 1] is AdvancedRepeat
                    && (i - 2 < 0 || !(nodes[i - 2] is AdvancedRepeat))) selectedID.Add(i);
            }
            if (selectedID.Count == 0) return false;
            int idx = selectedID[ran.Next(0, selectedID.Count)];

            int cnt = 3;
            int mul = int.MaxValue;
            int[] nTimes = new int[cnt];
            while (mul > 1000)
            {
                mul = 1;
                for (int i = 0; i < cnt; i++)
                {
                    nTimes[i] = ran.Next(0, 2);
                    if (nTimes[i] > 0)
                    {
                        nTimes[i] = ran.Next(2, 9);
                    }
                    else
                    {
                        nTimes[i] = ran.Next(30, 101);
                    }
                    mul *= nTimes[i];
                }
            }

            for (int i = 0; i < cnt; i++)
            {
                nodes.Insert(idx - 1, new AdvancedRepeat() { Times = nTimes[i] });
            }
            return true;
        }

        private bool SetVR()
        {
            List<int> selectedBulletID = new List<int>();
            for (int i = 1; i < nodes.Count; i++)
            {
                if (nodes[i] is SimpleBullet && nodes[i - 1] is AdvancedRepeat) selectedBulletID.Add(i);
            }
            if (selectedBulletID.Count == 0) return false;
            int idx = selectedBulletID[ran.Next(0, selectedBulletID.Count)];
            int arlb = idx - 1;
            while (arlb >= 0 && nodes[arlb] is AdvancedRepeat)
            {
                arlb--;
            }
            arlb++;
            SimpleBullet sb = nodes[idx] as SimpleBullet;
            List<int> idxs = new List<int>();
            for(int i = arlb; i < idx; i++)
            {
                if ((nodes[i] as AdvancedRepeat).Variables.Count == 0) idxs.Add(i);
            }
            AdvancedRepeat ar;
            if (idxs.Count != 0)
            {
                ar = nodes[idxs[ran.Next(0, idxs.Count)]] as AdvancedRepeat;
            }
            else
            {
                ar = nodes[ran.Next(arlb, idx)] as AdvancedRepeat;
            }

            string varName = $"{varCount}";

            Variable v = null;
            double lb, ub;
            if (ran.Next(0, 2) > 0)
            {
                //v
                lb = Convert.ToUInt64(ran.NextReversedDecayedDouble(0.125, 2) * 1000) / 1000.0;
                ub = Convert.ToUInt64(ran.NextDecayedDouble(0.125, 2) * 1000) / 1000.0;
                varName = "v_" + varName;
                sb.Velocity.Add(varName);
            }
            else
            {
                //r
                lb = 0;
                ub = ran.Next(0, 721) / 2;
                varName = "a_" + varName;
                sb.Angle.Add(varName);
            }
            v = new Variable() { Name = varName, InterpolationType = (InterpolationType)ran.Next(1, 8) };
            if (ran.Next(0, 2) > 0)
            {
                v.Begin = lb;
                v.End = ub;
            }
            else
            {
                v.Begin = ub;
                v.End = lb;
            }

            ar.Variables.Add(v);

            varCount++;
            return true;
        }

        private bool SetWait()
        {
            List<int> selectedBulletID = new List<int>();
            for (int i = 1; i < nodes.Count; i++)
            {
                if (nodes[i] is SimpleBullet && nodes[i - 1] is AdvancedRepeat) selectedBulletID.Add(i);
            }
            if (selectedBulletID.Count == 0) return false;
            int idx = selectedBulletID[ran.Next(0, selectedBulletID.Count)];
            int arlb = idx - 1;
            while (arlb >= 0 && nodes[arlb] is AdvancedRepeat)
            {
                arlb--;
            }
            arlb++;
            List<int> idxs = new List<int>();
            for (int i = arlb; i < idx; i++)
            {
                if ((nodes[i] as AdvancedRepeat).Wait == 0) idxs.Add(i);
            }
            if (idxs.Count != 0)
            {
                (nodes[idxs[ran.Next(0,idxs.Count)]] as AdvancedRepeat).Wait = ran.Next(0, 6);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal TreeNodeBase GetTreeNodes(DocumentData documentData)
        {
            TreeNodeBase root = null;
            TreeNodeBase curr = null;
            int currpos = 0;
            foreach (Node n in nodes)
            {
                TreeNodeBase t = n.GetTreeNode(documentData);
                if (root == null)
                {
                    root = t;
                }
                else
                {
                    curr.InsertChild(t, currpos);
                }
                if (n is AdvancedRepeat)
                {
                    curr = t;
                    currpos = 1;
                }
            }

            BossDefine newDef = new BossDefine(documentData);
            newDef.Name = "random";
            TreeNodeBase init = new BossInit(documentData);
            BossSpellCard newSC = new BossSpellCard(documentData);
            newSC.Name = $"CARD_{seed}";
            TreeNodeBase newSCStart = new BossSCStart(documentData);
            TreeNodeBase newTask = new TaskNode(documentData);
            newSCStart.AddChild(newTask);
            newTask.AddChild(new TaskMoveTo(documentData, "0,120", "60", "MOVE_NORMAL"));

            newTask.AddChild(root);

            newSC.AddChild(newSCStart);
            newSC.AddChild(new BossSCFinish(documentData));
            newDef.AddChild(init);
            newDef.AddChild(newSC);

            return newDef;
        }
    }
}
