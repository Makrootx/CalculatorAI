using CalculatorAI.CoreAI;
using CalculatorAI.MVVM.Core;
using CalculatorAI.MVVM.Views;
using CalculatorAI.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CalculatorAI.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        MainView MainVM {  get; set; }

        CalculatorView CalculatorVM { get; set; }

        BatleMainScreen BattleMainVM { get; set; }

        BattleScreen BattleVM { get; set; }

        BattleConclusionScreen BattleConclusionVM { get; set; }

        BattleGameOverScreen BattleGameOverVM { get; set; }

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
        public RelyCommand changeToBattleMainView;
        public RelyCommand changeToBattleView;
        public RelyCommand changeToBattleConclusionView;
        public RelyCommand changeToBattleViewFromConclusion;
        public RelyCommand changeToBattleGameOverView;


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
        ListBox toolBar;
        public MainViewModel(ListBox toolBar)
        {
            this.toolBar=toolBar;
            myModel = new MyModel("saved_model4");
            PredictionService.Model = myModel;
            MainVM = new MainView();
            CalculatorVM = new CalculatorView();
            CurrentView = MainVM;
            BattleMainVM=new BatleMainScreen();
            changeToMainView = new RelyCommand(o =>
            {
                CurrentView = MainVM;
            });
            changeToCalculatorView = new RelyCommand(o =>
            {
                CurrentView = CalculatorVM;
            });
            changeToBattleMainView = new RelyCommand(o =>
            {
                CurrentView = BattleMainVM;
            });
            changeToBattleView = new RelyCommand(o =>
            {
                BattleVM = new BattleScreen();
                CurrentView = BattleVM;
            });
            changeToBattleConclusionView = new RelyCommand(o =>
            {
                BattleConclusionVM=new BattleConclusionScreen((BattleInfo)o);
                CurrentView = BattleConclusionVM;
            });
            changeToBattleViewFromConclusion = new RelyCommand(o =>
            {
                BattleVM=new BattleScreen((BattleInfo)o);
                CurrentView = BattleVM;
            });
            changeToBattleGameOverView = new RelyCommand(o =>
            {
                BattleGameOverVM=new BattleGameOverScreen((BattleInfo)o);
                CurrentView = BattleGameOverVM;
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

        public void changeToolbarIndex(int index)
        {
            toolBar.SelectedIndex = index;
        }
    }
}

