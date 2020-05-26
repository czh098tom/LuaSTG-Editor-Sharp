using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmakuRandomizer.Model
{
    internal abstract class Variable
    {
        public string Name { get; set; }
        public InterpolationType InterpolationType { get; set; }
        public double Begin { get; set; }
        public double End { get; set; }
    }
}
