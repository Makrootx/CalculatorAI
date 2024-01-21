using CalculatorAI.MVVM.ViewModels;
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
using Tensorflow;

namespace CalculatorAI.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для CalculatorView.xaml
    /// </summary>
    public partial class CalculatorView : UserControl
    {
        MainViewModel mainViewModel;
        public CalculatorView()
        {
            InitializeComponent();
            mainViewModel = (MainViewModel)DataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            calculatingStack.Children.Clear();
            var prediction=getPredictionFromInkCanvas(Drawing_Canvas);
            createPredictBlockAndAnimation(prediction, () =>
            {
                prediction =getPredictionFromInkCanvas(Drawing_Canvas2);
                createPredictBlockAndAnimation(prediction, () =>
                {
                    prediction = getPredictionFromInkCanvas(Drawing_Canvas3);
                    createPredictBlockAndAnimation(prediction);
                });
            });
            
        }

        private string getPredictionFromInkCanvas(InkCanvas canvas)
        {
            var bitmap = getBitmapFromInkCanvas(canvas);
            var processedImages = CropingService.GetImages(bitmap);
            var prediction = ((MainViewModel)DataContext).getPrediction(processedImages);
            prediction = convertPrediction(prediction);
            return prediction;
        }

        private Bitmap getBitmapFromInkCanvas(InkCanvas drawingCanvas)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)drawingCanvas.ActualWidth, (int)drawingCanvas.ActualHeight, 96d, 96d, PixelFormats.Default);
            renderBitmap.Render(drawingCanvas);
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

        private Button addPredictBlock(string content)
        {
            Button button = new Button();
            TranslateTransform translateTransform = new TranslateTransform(0, 30);
            button.RenderTransform = translateTransform;
            button.Style = (Style)Application.Current.Resources["PredictBlock"];
            button.Content = content;
            calculatingStack.Children.Add(button);
            return button;
        }

        private void createPredictBlockAndAnimation(string content, Action onCompleted = null)
        {
            Button button = addPredictBlock(content);
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(1));
            doubleAnimation.Completed += (sender, e)=>onCompleted?.Invoke();
            button.RenderTransform.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);
        }

        private string convertPrediction(string prediction)
        {
            string ans="";
            foreach (var item in prediction.Split(' '))
            {
                switch (item)
                {
                    case "10":
                        ans += "+";
                        break;
                    case "11":
                        ans += "-";
                        break;
                    case "12":
                        ans += "*";
                        break;
                    case "13":
                        ans += "/";
                        break;
                    default:
                        ans+=item;
                        break;
                }
            }
            return ans;
            
        }
    }
}
