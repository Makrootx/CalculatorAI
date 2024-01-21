using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.Textures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;

namespace CalculatorAI.Services
{
    public class Test
    {
        public static int CountDigits(string imagePath)
        {
            // Load the image
            var image = AForge.Imaging.Image.FromFile(imagePath);
            

            // Convert the image to grayscale
            //var grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);

            var grayFilter = new Grayscale(0.7, 0.2, 0.1);
            var grayImage = grayFilter.Apply(image);

            var invertFilter = new Invert();
            invertFilter.ApplyInPlace(grayImage);

            // Apply thresholding to create a binary image
            var thresholdFilter = new Threshold(128);
            var binaryImage = thresholdFilter.Apply(grayImage);

            // Create BlobCounter to count connected components (blobs)
            var blobCounter = new BlobCounter();

            // Filter blobs based on size (area)
            blobCounter.MinWidth = 1;
            blobCounter.MinHeight = 1;
            blobCounter.FilterBlobs = true;

            // Process the binary image
            blobCounter.ProcessImage(binaryImage);
            var blobs = blobCounter.GetObjectsInformation();

            // Display the image with bounding boxes around blobs (digits)
            var imageWithBoundingBoxes = new Bitmap(image);  // Create a copy of the original image
            using (Graphics g = Graphics.FromImage(imageWithBoundingBoxes))
            {
                foreach (var blob in blobs)
                {
                    var rect = blob.Rectangle;
                    // Draw a red rectangle around the detected digit
                    g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), rect);
                }
            }

            // Save the image with bounding boxes
            imageWithBoundingBoxes.Save("output_image.png");

            // Return the count of detected digits
            return blobs.Length;
        }

        public static int CountDigits(Bitmap image_orig)
        {
            // Load the image
            var image = AForge.Imaging.Image.Clone(image_orig);


            // Convert the image to grayscale
            //var grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);

            var grayFilter = new Grayscale(0.7, 0.2, 0.1);
            var grayImage = grayFilter.Apply(image);

            var invertFilter = new Invert();
            invertFilter.ApplyInPlace(grayImage);

            // Apply thresholding to create a binary image
            var thresholdFilter = new Threshold(128);
            var binaryImage = thresholdFilter.Apply(grayImage);

            // Create BlobCounter to count connected components (blobs)
            var blobCounter = new BlobCounter();

            // Filter blobs based on size (area)
            blobCounter.MinWidth = 1;
            blobCounter.MinHeight = 1;
            blobCounter.FilterBlobs = true;

            // Process the binary image
            blobCounter.ProcessImage(binaryImage);
            var blobs = blobCounter.GetObjectsInformation();

            // Display the image with bounding boxes around blobs (digits)
            var imageWithBoundingBoxes = new Bitmap(image);  // Create a copy of the original image
            using (Graphics g = Graphics.FromImage(imageWithBoundingBoxes))
            {
                foreach (var blob in blobs)
                {
                    var rect = blob.Rectangle;
                    // Draw a red rectangle around the detected digit
                    g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), rect);
                }
            }

            // Save the image with bounding boxes
            imageWithBoundingBoxes.Save("output_image.png");

            // Return the count of detected digits
            return blobs.Length;
        }

        public static Bitmap GetImage(Bitmap image_orig)
        {
            // Load the image
            var image = AForge.Imaging.Image.Clone(image_orig);


            // Convert the image to grayscale
            //var grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);

            var grayFilter = new Grayscale(0.7, 0.2, 0.1);
            var grayImage = grayFilter.Apply(image);

            var invertFilter = new Invert();
            invertFilter.ApplyInPlace(grayImage);

            // Apply thresholding to create a binary image
            var thresholdFilter = new Threshold(128);
            var binaryImage = thresholdFilter.Apply(grayImage);

            // Create BlobCounter to count connected components (blobs)
            var blobCounter = new BlobCounter();

            // Filter blobs based on size (area)
            blobCounter.MinWidth = 20;
            blobCounter.MinHeight = 20;
            blobCounter.FilterBlobs = true;

            // Process the binary image
            blobCounter.ProcessImage(binaryImage);
            var blobs = blobCounter.GetObjectsInformation();

            // Display the image with bounding boxes around blobs (digits)
            var imageWithBoundingBoxes = new Bitmap(image);  // Create a copy of the original image
            using (Graphics g = Graphics.FromImage(imageWithBoundingBoxes))
            {
                foreach (var blob in blobs)
                {
                    var rect = blob.Rectangle;
                    // Draw a red rectangle around the detected digit
                    g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), rect);
                }
            }

            // Save the image with bounding boxes
            imageWithBoundingBoxes.Save("output_image.png");
            return imageWithBoundingBoxes;

            // Return the count of detected digits
            
        }
    }
}
