using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tensorflow.Keras.Engine;
using Tensorflow.NumPy;
using static Tensorflow.KerasApi;

namespace CalculatorAI.CoreAI
{
    public class MyModel
    {
        private string modelDir = "C:\\Users\\maksy\\source\\repos\\ModelAI\\ModelAI\\bin\\Debug\\net6.0\\";
        public IModel model;
        private int debugValue = 0;
        public MyModel(string modelName) {
            loadModelFromDir(modelName);
        }

        private void loadModelFromDir(string modelName)
        {
            model=keras.models.load_model(modelDir + modelName);
            model.compile(keras.optimizers.Adam(),
                keras.losses.SparseCategoricalCrossentropy(from_logits: true), metrics: new[] { "accuracy" });
        }

        public string getPredictions(Bitmap[] bitmaps)
        {
            string ans = "";
            debugValue = 0;
            bool first = false;
            foreach (Bitmap img in bitmaps)
            {
                if (first)
                {
                    ans=ans+" "; 
                }
                NDArray input = prepareInput(img);
                for (int i = 0; i < 28; i++)
                {
                    for (int j = 0; j < 28; j++)
                    {
                        if ((float)input[i, j, 0] > 0)
                        {

                            Console.Write("1");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
                input = input.reshape((1, 28, 28, 1));
                var prediction = model.predict(input);
                prediction = np.argmax(prediction.numpy()[0], -1);
                ans = ans + prediction.ToString();
                first = true;
            }
            return ans;
        }

        private NDArray prepareInput(Bitmap bitmap)
        {
            NDArray input = new NDArray((28, 28, 1), dtype: Tensorflow.TF_DataType.TF_FLOAT);
            Bitmap scaledDown = scaleDownBitmap(bitmap, 28, 28);
            scaledDown.Save("outputs/outputScaled" + debugValue++ + ".png");
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    System.Drawing.Color pixel = scaledDown.GetPixel(i, j);
                    if (pixel.A < 255)
                    {
                        pixel = System.Drawing.Color.FromArgb(255, 255, (255 - pixel.G), (255 - pixel.B));
                    }
                    pixel = System.Drawing.Color.FromArgb(255, (255 - pixel.R), (255 - pixel.G), (255 - pixel.B));

                    float pixelValue = pixel.R / 255.0f;

                    input[j, i, 0] = pixelValue;
                }
            }
            return input;
        }

        private Bitmap scaleDownBitmap(Bitmap bitmap, int newWidth, int newHeight)
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
