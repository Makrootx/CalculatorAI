using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace CalculatorAI.Styles
{
    public class CardElementProperties
    {
        public static readonly DependencyProperty MyImageSourceProperty =
            DependencyProperty.RegisterAttached("MyImageSource", typeof(ImageSource), typeof(CardElementProperties));

        public static void SetMyImageSource(DependencyObject d, ImageSource source)
        {
            d.SetValue(MyImageSourceProperty, source);
        }

        public static ImageSource GetMyImageSource(DependencyObject dp)
        {
            return (ImageSource)dp.GetValue(MyImageSourceProperty);
        }
    }
}
