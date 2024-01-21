using CalculatorAI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorAI.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для PlayGroundView.xaml
    /// </summary>
    public partial class PlayGroundView : UserControl
    {
        public PlayGroundView()
        {
            InitializeComponent();
        }

        private void OutlinedComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sizeStr = brushSizeCombobox.SelectedItem.ToString();
            int size = int.Parse(sizeStr.Split(' ')[1]);

            changeBrushSize(size);
        }

        private void changeBrushSize(int newSize)
        {
            DrawingAttributes drawingAttributes = Drawing_Canvas.DefaultDrawingAttributes;

            // Change properties as needed
            // Change pen color
            drawingAttributes.Width = newSize;                 // Change pen thickness
            drawingAttributes.Height = newSize;                 // Change pen height
            drawingAttributes.FitToCurve = true;          // Enable or disable fit-to-curve behavior
            //drawingAttributes.StylusTip = StylusTip.Ellipse;
            Drawing_Canvas.DefaultDrawingAttributes=drawingAttributes;

        }

        

        private void changeBrushToEraser(DrawingAttributes drawingAttributes)
        {
            System.Windows.Media.Color transparentColor = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);

            var size = drawingAttributes.Width;
            // Set the DrawingAttributes to use the transparent color
            DrawingAttributes eraserAttributes = new DrawingAttributes
            {
                Color = transparentColor,
                StylusTip = StylusTip.Rectangle, // You can use other tip shapes if desired
                Width = size,  // Set the width of the eraser
                Height = size  // Set the height of the eraser
            };

            Drawing_Canvas.DefaultDrawingAttributes = drawingAttributes;
        }

        private void changeBrushToPen(DrawingAttributes drawingAttributes)
        {
            var size = drawingAttributes.Width;
            // Set the DrawingAttributes to use the transparent color
            DrawingAttributes eraserAttributes = new DrawingAttributes
            {
                Color = Colors.Black,
                StylusTip = StylusTip.Ellipse, // You can use other tip shapes if desired
                Width = size,  // Set the width of the eraser
                Height = size  // Set the height of the eraser
            };

            Drawing_Canvas.DefaultDrawingAttributes = drawingAttributes;
        }

        private void changeToPen_Checked(object sender, RoutedEventArgs e)
        {
            changeBrushToPen(Drawing_Canvas.DefaultDrawingAttributes);
        }

        private void changeToEraser_Checked(object sender, RoutedEventArgs e)
        {
            changeBrushToEraser(Drawing_Canvas.DefaultDrawingAttributes);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            calculatingStack.Children.Clear();
            string prediction = PredictionService.getPredictionFromInkCanvas(Drawing_Canvas);
            createPredictBlockAndAnimation(prediction);

        }

        private Button addPredictBlock(string content, SolidColorBrush color)
        {
            Button button = new Button();
            TranslateTransform translateTransform = new TranslateTransform(0, 30);
            button.RenderTransform = translateTransform;
            button.Style = (Style)Application.Current.Resources["PredictBlock"];
            button.Content = content;
            if (color == null)
                button.Background = new SolidColorBrush(Colors.White);
            else
                button.Background = color;
            calculatingStack.Children.Add(button);
            return button;
        }

        private void createPredictBlockAndAnimation(string content, SolidColorBrush color = null)
        {
            Button button = addPredictBlock(content, color);
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            button.RenderTransform.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Drawing_Canvas.Strokes.Clear();
        }
    }
}
