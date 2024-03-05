using AForge.Imaging.Filters;
using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAI.Services
{
    public class CropingService
    {
        public static Bitmap GetImage(Bitmap image_orig)
        {

            var image = AForge.Imaging.Image.Clone(image_orig);
            var binaryImage=getBinaryProcessedImage(image);
            var blobs = getBlobsFromProcessedImage(binaryImage);

            
            var imageWithBoundingBoxes = new Bitmap(image);
            using (Graphics g = Graphics.FromImage(imageWithBoundingBoxes))
            {
                foreach (var blob in blobs)
                {
                    var rect1 = blob.Rectangle;
                    g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), rect1);
                }
            }

            var maxBlob = blobs.First(p => p.Rectangle.Size == blobs.Max(o => o.Rectangle.Size));

            var addValue = (int)(Math.Sqrt(maxBlob.Rectangle.Width * maxBlob.Rectangle.Height) / 10);
            addValue = 0;
            Rectangle rect;


            rect = new Rectangle(maxBlob.Rectangle.X, maxBlob.Rectangle.Y, maxBlob.Rectangle.Width, maxBlob.Rectangle.Height);


            var adjustValue = Math.Abs(rect.Width - rect.Height);

            Bitmap imagePreprocessed;

            if (rect.Width > rect.Height)
            {
                imagePreprocessed = new Bitmap(maxBlob.Rectangle.Width + addValue, maxBlob.Rectangle.Height + adjustValue);
            }
            else
            {
                imagePreprocessed = new Bitmap(maxBlob.Rectangle.Width + adjustValue, maxBlob.Rectangle.Height + addValue);
            }


            using (Graphics g = Graphics.FromImage(imagePreprocessed))
            {
                var supRect = new Rectangle(0, 0, imagePreprocessed.Width, imagePreprocessed.Height);
                var brush = new SolidBrush(System.Drawing.Color.White);
                g.FillRectangle(brush, supRect);
                if (rect.Width > rect.Height)
                {
                    g.DrawImage(image, new Rectangle(addValue / 2, adjustValue / 2, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);

                }
                else
                {
                    g.DrawImage(image, new Rectangle(adjustValue / 2, addValue / 2, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);
                }
            }
            return imagePreprocessed;
        }

        public static Bitmap[] GetImages(Bitmap imag_orig)
        {
            var binaryImage = getBinaryProcessedImage(imag_orig);

            var blobs = getBlobsFromProcessedImage(binaryImage);

            var blobesSorted = blobs.OrderBy(p => p.Rectangle.X);

            List<Bitmap> imagesProcessed = new List<Bitmap>();

            int i = 0;

            foreach (var blob in blobesSorted)
            {
                Rectangle rect;

                rect = new Rectangle(blob.Rectangle.X, blob.Rectangle.Y, blob.Rectangle.Width, blob.Rectangle.Height);

                var adjustValue = Math.Abs(rect.Width - rect.Height);
                Bitmap imageProcessed;

                var adjustWidth = (rect.Width > rect.Height) ? 0 : adjustValue;
                var adjustHeight = (rect.Height > rect.Width) ? 0 : adjustValue;

                imageProcessed = new Bitmap(rect.Width + adjustWidth, rect.Height + adjustHeight);

                using (Graphics g = Graphics.FromImage(imageProcessed))
                {
                    var supRect = new Rectangle(0, 0, imageProcessed.Width, imageProcessed.Height);
                    var brush = new SolidBrush(System.Drawing.Color.White);
                    g.FillRectangle(brush, supRect);
                    g.DrawImage(imag_orig, new Rectangle(adjustWidth / 2, adjustHeight / 2, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);
                }
                i++;
                imagesProcessed.Add(imageProcessed);
            }
            return imagesProcessed.ToArray();
        }

        private static Bitmap getBinaryProcessedImage(Bitmap originBitmap)
        {
            var image = AForge.Imaging.Image.Clone(originBitmap);

            var grayFiletr = new Grayscale(0.7, 0.2, 0.1);
            var grayImage = grayFiletr.Apply(image);

            var invertFilter = new Invert();
            var invertedImage = invertFilter.Apply(grayImage);

            var thresholdFilter = new Threshold(128);
            var binaryImage = thresholdFilter.Apply(invertedImage);
            return binaryImage;
        }

        private static Blob[] getBlobsFromProcessedImage(Bitmap binaryImage)
        {
            var blobCounter = new BlobCounter();

            blobCounter.MinWidth = 20;
            blobCounter.MinHeight = 10;
            blobCounter.FilterBlobs = true;

            blobCounter.ProcessImage(binaryImage);

            var blobs = blobCounter.GetObjectsInformation();
            return blobs;
        }
    }
}
