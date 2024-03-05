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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Ink;
using System.Drawing;
using System.IO;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using CalculatorAI.MVVM.ViewModels;

namespace CalculatorAI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel;
        bool isColapsed = false;
        public MainWindow()
        {

            InitializeComponent();
            //Drawing_Canvas.EditingMode = InkCanvasEditingMode.Ink;
            viewModel = new MainViewModel(ToolBarItems);
            DataContext = viewModel;
            ToolBarItems.SelectedIndex = 0;

        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            if (HamburgerButton.IsChecked==false) { 
                DoubleAnimation anim = new DoubleAnimation(0, TimeSpan.FromSeconds(0.7));
                ToolBar.BeginAnimation(StackPanel.WidthProperty, anim);
                DoubleAnimation animForHamburger = new DoubleAnimation(50, TimeSpan.FromSeconds(0.7));
                pomp.BeginAnimation(Border.WidthProperty, animForHamburger);
            }
            else
            {
                DoubleAnimation anim = new DoubleAnimation(150, TimeSpan.FromSeconds(0.7));
                ToolBar.BeginAnimation(StackPanel.WidthProperty, anim);
                DoubleAnimation animForHamburger = new DoubleAnimation(0, TimeSpan.FromSeconds(0.7));
                pomp.BeginAnimation(Border.WidthProperty, animForHamburger);
            }
            //isColapsed = (isColapsed)? false: true;
            
        }

        private void closeToolBar()
        {
            DoubleAnimation anim = new DoubleAnimation(0, TimeSpan.FromSeconds(0.7));
            ToolBar.BeginAnimation(StackPanel.WidthProperty, anim);
            DoubleAnimation animForHamburger = new DoubleAnimation(50, TimeSpan.FromSeconds(0.7));
            pomp.BeginAnimation(Border.WidthProperty, animForHamburger);
            HamburgerButton.IsChecked = false;
        }

        private void ToolBarItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ToolBarItems.SelectedIndex)
            {
                case 0:
                    viewModel.changeToMainView.Execute(null);
                    closeToolBar();
                    break;
                case 1:
                    viewModel.changeToCalculatorView.Execute(null);
                    closeToolBar();
                    break;
                case 2:
                    viewModel.changeToBattleMainView.Execute(null);
                    closeToolBar();
                    break;
                case 3:
                    viewModel.changeToPlaygroundView.Execute(null);
                    closeToolBar();
                    break;
            }
        }

        private void CloseBut_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ResizeBut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MinimizeBut_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    StrokeCollection stroke = Drawing_Canvas.Strokes;
        //    RenderTargetBitmap renderBitmap= new RenderTargetBitmap((int)Drawing_Canvas.ActualWidth, (int)Drawing_Canvas.ActualHeight, 96d, 96d, PixelFormats.Default);
        //    renderBitmap.Render(Drawing_Canvas);
        //    BitmapSource bitmapSource = renderBitmap;
        //    Bitmap bitmap;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
        //        pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));
        //        pngEncoder.Save(ms);
        //        bitmap = new Bitmap(ms);
        //    }

        //    Bitmap procesedBitmap=Test.GetImage(bitmap);

        //    BitmapSource image=Imaging.CreateBitmapSourceFromHBitmap(procesedBitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        //    result_image.Source= image;
        //}
    }
}
