using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LuaSTGEditorSharp.Windows.Input.Canvas
{
    public class DrawingCanvas : System.Windows.Controls.Canvas
    {
        private List<Visual> visuals = new List<Visual>();
        protected override int VisualChildrenCount => visuals.Count;

        public int ChildrenCount => visuals.Count;

        protected override Visual GetVisualChild(int index) => visuals[index];

        public Visual this[int index]
        {
            get => visuals[index];
            set
            {
                visuals[index] = value;
            }
        }

        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }
        
        public void RemoveVisual(Visual visual)
        {
            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        public static double ScrXToLSTGX(double x)
        {
            return x - 224;
        }

        public static double ScrYToLSTGY(double y)
        {
            return 240 - y;
        }

        public static double ScrXToLSTGX(double x, bool? clip10)
        {
            if (clip10 == null)
            {
                return x - 224;
            }
            else if (clip10 == false)
            {
                return Convert.ToInt32(x - 224);
            }
            else
            {
                return Convert.ToInt32((x - 224) / 10) * 10;
            }
        }

        public static double ScrYToLSTGY(double y, bool? clip10)
        {
            if (clip10 == null)
            {
                return 240 - y;
            }
            else if (clip10 == false)
            {
                return Convert.ToInt32(240 - y);
            }
            else
            {
                return Convert.ToInt32((240 - y) / 10) * 10;
            }
        }

        public static double LSTGXToScrX(double x)
        {
            return x + 224;
        }

        public static double LSTGYToScrY(double y)
        {
            return 240 - y;
        }
    }
}
