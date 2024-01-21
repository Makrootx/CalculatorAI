using CalculatorAI.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace CalculatorAI.Services
{
    public class BitmapService
    {
        public static Bitmap getBitmapFromInkCanvas(InkCanvas drawingCanvas)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)drawingCanvas.ActualWidth, (int)drawingCanvas.ActualHeight, 96d, 96d, PixelFormats.Default);
            renderBitmap.Render(drawingCanvas);
            //BitmapSource bitmapSource = renderBitmap;
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

        public static Bitmap scaleDownBitmap(Bitmap bitmap, int newWidth, int newHeight)
        {
            Bitmap scaledDown = new Bitmap(newWidth, newHeight);
            bitmap.Save("originalBitmap.png");
            using (Graphics g = Graphics.FromImage(scaledDown))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, 0, 0, newWidth, newHeight);
            }
            scaledDown.Save("scaledDownImage.png");
            return scaledDown;
        }
    }
}
