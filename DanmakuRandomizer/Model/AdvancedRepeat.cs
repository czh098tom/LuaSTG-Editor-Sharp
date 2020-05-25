using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmakuRandomizer.Model
{
    internal class AdvancedRepeat : Node
    {
        public string Times { get; set; }
        public string Wait { get; set; }
        public override string Text => $"repeat {Times} times, wait {Wait} frames";
    }
}
