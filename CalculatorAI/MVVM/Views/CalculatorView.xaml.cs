using CalculatorAI.MVVM.ViewModels;
using CalculatorAI.Services;
using DocumentFormat.OpenXml.Office2010.Drawing;
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
using System.Windows.Ink;
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

        SolidColorBrush defaultColor;
        SolidColorBrush errorColor;
        SolidColorBrush defaultNumberColor;
        SolidColorBrush defaultOperationColor;

        int[] sizeMap = new int[] { 5, 7, 10, 12 };
        public CalculatorView()
        {
            InitializeComponent();
            mainViewModel = (MainViewModel)DataContext;
            defaultColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
            errorColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
            defaultNumberColor = new SolidColorBrush(System.Windows.Media.Colors.PaleVioletRed);
            defaultOperationColor = new SolidColorBrush(System.Windows.Media.Colors.PaleGreen);

            changeToPen.IsChecked = true;
            brushSizeCombobox.SelectedIndex = 0;
        }

        private void GetAnswerBut_Click(object sender, RoutedEventArgs e)
        {
            GetAnswerBut.IsEnabled = false;
            calculatingStack.Children.Clear();
            string[] predictions=new string[3];
            var prediction=PredictionService.getPredictionFromInkCanvas(Drawing_Canvas);
            predictions[0] = prediction;
            SolidColorBrush color;
            color = (PredictionService.isNumber(prediction)) ? defaultNumberColor : errorColor;
            createPredictBlockAndAnimation(prediction, color, () =>
            {
                prediction = PredictionService.getPredictionFromInkCanvas(Drawing_Canvas2);
                predictions[1] = prediction;
                color = (PredictionService.isOperation(prediction)) ? defaultOperationColor : errorColor;
                createPredictBlockAndAnimation(prediction, color, () =>
                {
                    prediction = PredictionService.getPredictionFromInkCanvas(Drawing_Canvas3);
                    predictions[2] = prediction;
                    color = (PredictionService.isNumber(prediction)) ? defaultNumberColor : errorColor;
                    createPredictBlockAndAnimation(prediction, color, () =>
                    {
                        try
                        {
                            double ans=makeCalculation(predictions);
                            createPredictBlockAndAnimation("=", defaultOperationColor, () =>
                            {
                                createPredictBlockAndAnimation(ans.ToString(), defaultNumberColor);
                                GetAnswerBut.IsEnabled = true;
                            });
                            
                        }
                        catch (ArgumentException ex)
                        {
                            GetAnswerBut.IsEnabled = true;
                        }
                    });
                });
            });
            
        }

        private double makeCalculation(string[] predictions)
        {
            try
            {
                int a = PredictionService.getNumberFromPrediction(predictions[0]);
                int b = PredictionService.getNumberFromPrediction(predictions[2]);
                double ans;
                switch (predictions[1])
                {
                    case "+":
                        ans = a + b;
                        break;
                    case "-":
                        ans = a - b;
                        break;
                    case "*":
                        ans = a * b;
                        break;
                    case "/":
                        ans = a / b;
                        break;
                    default: throw new ArgumentException("Operation is invalid");
                }
                return ans;
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
            
        }

        private Button addPredictBlock(string content, SolidColorBrush color)
        {
            Button button = new Button();
            TranslateTransform translateTransform = new TranslateTransform(0, 30);
            button.RenderTransform = translateTransform;
            button.Style = (Style)Application.Current.Resources["PredictBlock"];
            button.Content = content;
            if(color==null)
                button.Background = defaultColor;
            else
                button.Background = color;
            calculatingStack.Children.Add(button);
            return button;
        }

        private void createPredictBlockAndAnimation(string content, SolidColorBrush color=null, Action onCompleted = null)
        {
            Button button = addPredictBlock(content, color);
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            doubleAnimation.Completed += (sender, e)=>onCompleted?.Invoke();
            button.RenderTransform.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);
        }

        private void ClearCanvas3But_Click(object sender, RoutedEventArgs e)
        {
            clearCanvas(Drawing_Canvas3);
        }

        private void clearCanvas(InkCanvas inkCanvas)
        {
            inkCanvas.Strokes.Clear();
        }

        private void ClearCanvas2But_Click(object sender, RoutedEventArgs e)
        {
            clearCanvas(Drawing_Canvas2);
        }

        private void ClearCanvas1But_Click(object sender, RoutedEventArgs e)
        {
            clearCanvas(Drawing_Canvas);
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
            drawingAttributes.Width = newSize;
            drawingAttributes.Height = newSize;
            drawingAttributes.FitToCurve = true;
            setBrushToAllCanvas(drawingAttributes);
            
        }

        private void setBrushToAllCanvas(DrawingAttributes drawingAttributes)
        {
            Drawing_Canvas.DefaultDrawingAttributes = drawingAttributes;
            Drawing_Canvas2.DefaultDrawingAttributes = drawingAttributes;
            Drawing_Canvas3.DefaultDrawingAttributes = drawingAttributes;
        }

        private void changeBrushToEraser(DrawingAttributes drawingAttributes)
        {
            System.Windows.Media.Color transparentColor = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);

            var size= drawingAttributes.Width;
            DrawingAttributes eraserAttributes = new DrawingAttributes
            {
                Color = transparentColor,
                StylusTip = StylusTip.Rectangle,
                Width = size,
                Height = size
            };

            setBrushToAllCanvas(eraserAttributes);
        }

        private void changeBrushToPen(DrawingAttributes drawingAttributes)
        {
            var size = drawingAttributes.Width;
            DrawingAttributes eraserAttributes = new DrawingAttributes
            {
                Color = Colors.Black,
                StylusTip = StylusTip.Ellipse,
                Width = size,
                Height = size
            };
            setBrushToAllCanvas(eraserAttributes);
        }

        private void changeToPen_Checked(object sender, RoutedEventArgs e)
        {
            changeBrushToPen(Drawing_Canvas.DefaultDrawingAttributes);
        }

        private void changeToEraser_Checked(object sender, RoutedEventArgs e)
        {
            changeBrushToEraser(Drawing_Canvas.DefaultDrawingAttributes);
        }

        

    }
    
}
