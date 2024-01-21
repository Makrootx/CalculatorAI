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

        PlayGroundView PlayGroundVM { get; set; }

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
        public RelyCommand changeToPlaygroundView;


        MyModel myModel;
        ListBox toolBar;

        private bool battleMode=false;
        public MainViewModel(ListBox toolBar)
        {
            this.toolBar=toolBar;
            myModel = new MyModel("saved_model4");
            PredictionService.Model = myModel;
            MainVM = new MainView();
            CalculatorVM = new CalculatorView();
            CurrentView = MainVM;
            BattleMainVM=new BatleMainScreen();
            PlayGroundVM=new PlayGroundView();
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
                battleMode = true;
            });
            changeToBattleConclusionView = new RelyCommand(o =>
            {
                BattleVM = null;
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
                BattleVM = null;
                BattleGameOverVM =new BattleGameOverScreen((BattleInfo)o);
                CurrentView = BattleGameOverVM;
            });
            changeToPlaygroundView = new RelyCommand(o =>
            {
                CurrentView = PlayGroundVM;
            });
        }

        public string getPrediction(Bitmap[] bitmaps)
        {
            return myModel.getPredictions(bitmaps);
        }

        public void changeToolbarIndex(int index)
        {
            toolBar.SelectedIndex = index;
            if (battleMode == true )
            {
                BattleVM = null;
            }
        }
    }
}

