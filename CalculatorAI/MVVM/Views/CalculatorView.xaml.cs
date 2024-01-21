using CalculatorAI.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorAI.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для CalculatorView.xaml
    /// </summary>
    public partial class CalculatorView : UserControl
    {
        public CalculatorView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Bitmap bitmap = getBitmapFromInkCanvas();

            Test.CountDigits(bitmap);
            addPredictBlock("167");
        }

        private Bitmap getBitmapFromInkCanvas()
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)Drawing_Canvas.ActualWidth, (int)Drawing_Canvas.ActualHeight, 96d, 96d, PixelFormats.Default);
            renderBitmap.Render(Drawing_Canvas);
            BitmapSource bitmapSource = renderBitmap;
            Bitmap bitmap;
            using (MemoryStream ms = new MemoryStream())
            {
                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                pngEncoder.Save(ms);
                bitmap = new Bitmap(ms);
            }
            return bitmap;
        }

        private void addPredictBlock(String content)
        {
            calculatingStack.Children.Clear();
            Button button = new Button();
            TranslateTransform translateTransform = new TranslateTransform(0, 30);
            button.RenderTransform = translateTransform;
            button.Style = (Style)Application.Current.Resources["PredictBlock"];
            button.Content = content;
            calculatingStack.Children.Add(button);
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(1));
            translateTransform.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);
        }
    }
}
