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
        public string Velocity { get; set; } = "4";
        public string Angle { get; set; } = "0";
        public List<string> PositionX { get; set; } = new List<string>() { "self.x" };
        public List<string> PositionY { get; set; } = new List<string>() { "self.y" };

        public override string Text => throw new NotImplementedException();
    }
}
