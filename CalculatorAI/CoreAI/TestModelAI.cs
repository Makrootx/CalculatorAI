using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tensorflow;
using Tensorflow.Keras;
using Tensorflow.Keras.Layers;
using Tensorflow.Keras.Engine;

namespace CalculatorAI.CoreAI
{
    public class TestModelAI
    {
        public static Model CreateModel()
        {
            var keras = KerasApi.keras;
            var layersApi= keras.layers;

            var model = keras.Sequential();

            model.add(layersApi.Conv2D(32, 3, activation: "relu"));
            model.add(layersApi.Dense(32, activation: "relu"));
            //model.add(KerasApi.keras.layers.

            model.compile(optimizer: "adam", loss: "sparse_categorical_crossentropy", metrics: new[] { "accuracy" });

            return model;
        }

        
    }
}
