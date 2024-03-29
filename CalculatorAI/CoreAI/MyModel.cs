﻿using CalculatorAI.Services;
using System;
using System.Drawing;
using System.IO;
using Tensorflow.Keras.Engine;
using Tensorflow.NumPy;
using static Tensorflow.KerasApi;

namespace CalculatorAI.CoreAI
{
    public class MyModel
    {
        private static string baseDirectory;
        public IModel model;
        private int debugValue = 0;
        public MyModel(string modelName)
        {
            loadModelFromDir(modelName);
        }

        private void loadModelFromDir(string modelName)
        {
            try
            {
                baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string modelDir = Path.Combine(baseDirectory, "public");
                model = keras.models.load_model(Path.Combine(modelDir, modelName));
            }
            catch (IOException e)
            {
                model = keras.models.load_model(modelName);
            }
            catch(Exception e)
            {
                model = keras.models.load_model(modelName);
            }
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
                    ans = ans + " ";
                }
                NDArray input = prepareInput(img);
                input = input.reshape((1, 28, 28, 1));
                var prediction = model.predict(input);
                prediction = np.argmax(prediction.numpy()[0], -1);
                ans = ans + prediction.ToString();
                first = true;
            }
            return ans;
        }

        private void debugOutput(NDArray input)
        {
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
        }

        private NDArray prepareInput(Bitmap bitmap)
        {
            NDArray input = new NDArray((28, 28, 1), dtype: Tensorflow.TF_DataType.TF_FLOAT);
            Bitmap scaledDown = BitmapService.scaleDownBitmap(bitmap, 28, 28);
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

                    if (pixel.R > 120)
                    {
                        pixel = System.Drawing.Color.FromArgb(255, 255, 0, 0);
                    }

                    float pixelValue = pixel.R / 255.0f;

                    input[j, i, 0] = pixelValue;
                }
            }
            return input;
        }
    }
}
