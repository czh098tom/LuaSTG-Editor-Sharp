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
        public string Times { get; set; } = "1";
        public string Wait { get; set; } = "0";
        public List<Variable> Variables { get; set; } = new List<Variable>();
        public override string Text => $"repeat {Times} times, wait {Wait} frames";
    }
}
