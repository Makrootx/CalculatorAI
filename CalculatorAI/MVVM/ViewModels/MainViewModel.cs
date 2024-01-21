using CalculatorAI.CoreAI;
using CalculatorAI.MVVM.Core;
using CalculatorAI.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAI.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        //OpenFileView OpenFileVM { get; set; }
        //StartingScreenView StartingScreenVM { get; set; }
        //SongSelectScreen SongSelectScreenVM { get; set; }

        //KaraokeScreen KaraokeScreenVM { get; set; }

        MainView MainVM {  get; set; }

        CalculatorView CalculatorVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;

                OnPropertyChanged();

            }
        }

        public RelyCommand changeToMainView;
        public RelyCommand changeToCalculatorView;


        public RelyCommand changeToKaraokeScreenView;
        public RelyCommand changeToSongSelectScreenView;
        public RelyCommand changeToKaraokeFromSong;
        //public RelyCommand changeToWriteFile;
        //public RelyCommand writeFileContent;
        //public RelyCommand readFileContent;

        //public bool isOpenFileVM()
        //{
        //    return CurrentView == OpenFileVM;
        //}

        MyModel myModel;
        public MainViewModel()
        {
            myModel = new MyModel("saved_model4");
            MainVM = new MainView();
            CalculatorVM = new CalculatorView();
            CurrentView = MainVM;
            changeToMainView = new RelyCommand(o =>
            {
                CurrentView = MainVM;
            });
            changeToCalculatorView = new RelyCommand(o =>
            {
                CurrentView = CalculatorVM;
            });

            //StartingScreenVM = new StartingScreenView(lyricsOperator);
            //SongSelectScreenVM = new SongSelectScreen(lyricsOperator);

            //    WriteFileVM = new WriteFileView();
            //CurrentView = StartingScreenVM;
            //changeToStartingScreenView = new RelyCommand(o =>
            //{
            //    CurrentView = StartingScreenVM;
            //});
        }

        public string getPrediction(Bitmap[] bitmaps)
        {
            return myModel.getPredictions(bitmaps);
        }
    }
}

