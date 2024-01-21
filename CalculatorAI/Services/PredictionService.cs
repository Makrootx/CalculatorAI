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
using CalculatorAI.CoreAI;

namespace CalculatorAI.Services
{
    public class PredictionService
    {

        private static MyModel model;
        public static MyModel Model { set { model = value; } }
        public static string getPredictionFromInkCanvas(InkCanvas canvas)
        {
            var bitmap = BitmapService.getBitmapFromInkCanvas(canvas);
            var processedImages = CropingService.GetImages(bitmap);
            var prediction = model.getPredictions(processedImages);
            prediction = convertPrediction(prediction);
            return prediction;
        }

        private static string convertPrediction(string prediction)
        {
            string ans = "";
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
                        ans += item;
                        break;
                }
            }
            return ans;
        }

        public static int getNumberFromPrediction(string prediction)
        {
            if (int.TryParse(prediction, out int number))
            {
                return number;
            }
            throw new ArgumentException("Prediction containing numbers");
        }

        public static bool isNumber(string prediction)
        {
            if (int.TryParse(prediction, out int number))
            {
                return true;
            }
            return false;
        }

        public static bool isOperation(string prediction)
        {
            switch (prediction)
            {
                case "+":
                    return true;
                case "-":
                    return true;
                case "*":
                    return true;
                case "/":
                    return true;
                default: return false;
            }
        }

    }
}
