using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmakuRandomizer.Model
{
    class SimpleBullet : Node
    {
        public string Velocity { get; set; }
        public string Angle { get; set; }
        public List<string> positionX = new List<string>();
        public List<string> positionY = new List<string>();

        public override string Text => throw new NotImplementedException();
    }
}
