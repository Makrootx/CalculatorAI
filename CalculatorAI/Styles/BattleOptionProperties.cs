using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace CalculatorAI.Styles
{
    public class BattleOptionProperties
    {
        public static readonly DependencyProperty MyPathDataProperty =
            DependencyProperty.RegisterAttached("MyPathData", typeof(Geometry), typeof(BattleOptionProperties));

        public static void SetMyPathData(DependencyObject d, Geometry value)
        {
            d.SetValue(MyPathDataProperty, value);
        }

        public static Geometry GetMyPathData(DependencyObject dp)
        {
            return (Geometry)dp.GetValue(MyPathDataProperty);
        }
    }
}
