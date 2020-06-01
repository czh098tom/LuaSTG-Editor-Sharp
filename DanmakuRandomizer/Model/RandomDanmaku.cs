using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmakuRandomizer.Model
{
    internal class RandomDanmaku
    {
        List<Node> nodes;
        Random ran;
        int varCount;
        public int Depth { get; set; }

        internal RandomDanmaku Randomize()
        {
            ran = new Random();
            varCount = 0;
            nodes = new List<Node>();
            nodes.Add(new AdvancedRepeat() { Times = "1" });
            nodes.Add(new SimpleBullet());
            for (int i = 0; i < Depth; i++)
            {
                switch (ran.Next(0, 8))
                {
                    case 0:
                        if (!AddAdvancedRepeat()) i--;
                        break;
                    case 1:
                        SetVR();
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

            int cnt = ran.Next(1, 4);
            int mul = int.MaxValue;
            int[] nTimes = new int[cnt];
            while (mul > 1000)
            {
                mul = 1;
                for (int i = 0; i < cnt; i++)
                {
                    nTimes[i] = ran.Next(0, 2) * 2 - 1;
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
                nodes.Insert(idx - 1, new AdvancedRepeat() { Times = nTimes[i].ToString() });
            }
            return true;
        }

        private void SetVR()
        {
            if (ran.Next() > 0)
            {
                //v
                double lb = Convert.ToUInt64(ran.NextReversedDecayedDouble(0.125, 1) * 1000) / 1000.0;
                double ub = Convert.ToUInt64(ran.NextDecayedDouble(0.125, 1) * 1000) / 1000.0;
                varCount++;
            }
            else
            {
                //r
            }
        }
    }
}
