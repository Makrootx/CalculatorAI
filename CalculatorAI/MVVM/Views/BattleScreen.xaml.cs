using CalculatorAI.MVVM.Core;
using CalculatorAI.MVVM.ViewModels;
using CalculatorAI.Services;
using CalculatorAI.Styles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CalculatorAI.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для BattleScreen.xaml
    /// </summary>
    public partial class BattleScreen : UserControl
    {
        Random random;
        private List<BattleQuestion> battleQuestions = new List<BattleQuestion>();
        private Geometry sword;
        private Geometry health;
        private List<StackPanel> questionsPanels = new List<StackPanel>();
        BattleQuestion battleQuestionToRemove;
        DispatcherTimer timer;

        private int userHealth=100;

        private int enemyHealth = 100;
        private int gameScore = 0;

        public BattleScreen(BattleInfo battleInfo=null)
        {
            InitializeComponent();
            if (battleInfo != null)
            {
                userHealth = battleInfo.health;
                gameScore = battleInfo.score;
            }
            random = new Random();
            sword = Geometry.Parse("M220.24219,35.75732A5.99827,5.99827,0,0,0,216,34h-.01855l-63.79883.20117a5.9993,5.9993,0,0,0-4.60742,2.17871L75.77808,123.292l-9.87867-9.8789a14.02057,14.02057,0,0,0-19.7998.001L33.415,126.09961a13.99961,13.99961,0,0,0-.001,19.7998l20.8877,20.8877a2.00282,2.00282,0,0,1,0,2.82812L24.36035,199.55664a14.015,14.015,0,0,0,0,19.79883l12.28418,12.28418a13.99963,13.99963,0,0,0,19.79883,0l29.94141-29.94141h.001a1.99809,1.99809,0,0,1,2.82715,0l20.8877,20.88867a14.02057,14.02057,0,0,0,19.7998-.001L142.585,209.90039a13.99961,13.99961,0,0,0,.001-19.7998l-9.87842-9.87842,86.9126-71.79737a5.99991,5.99991,0,0,0,2.17871-4.60693L222,40.01855A6.00065,6.00065,0,0,0,220.24219,35.75732Zm-86.1416,162.82959a1.99693,1.99693,0,0,1-.001,2.82715L121.415,214.09961a2.00222,2.00222,0,0,1-2.8291.001L97.69922,193.21338a13.999,13.999,0,0,0-19.79883-.00049L47.958,223.1543a1.99929,1.99929,0,0,1-2.82813,0L32.8457,210.87012a2.00193,2.00193,0,0,1-.001-2.82813l29.94238-29.9414a14.01674,14.01674,0,0,0,0-19.79883l-20.8877-20.88867a1.99693,1.99693,0,0,1,.001-2.82715L54.585,121.90039a2.004,2.004,0,0,1,2.83008-.001l14.50513,14.50538.01562.01562Zm75.707-97.62109L124.184,171.69824l-15.69861-15.69873,55.75684-55.75683a5.99971,5.99971,0,1,0-8.48438-8.48536l-55.75732,55.75733L84.30188,131.81592l70.7323-85.62354,54.94727-.17334Z");
            health = Geometry.Parse("M41.267,18.557H26.832V4.134C26.832,1.851,24.99,0,22.707,0c-2.283,0-4.124,1.851-4.124,4.135v14.432H4.141c-2.283,0-4.139,1.851-4.138,4.135c-0.001,1.141,0.46,2.187,1.207,2.934c0.748,0.749,1.78,1.222,2.92,1.222h14.453V41.27c0,1.142,0.453,2.176,1.201,2.922c0.748,0.748,1.777,1.211,2.919,1.211c2.282,0,4.129-1.851,4.129-4.133V26.857h14.435c2.283,0,4.134-1.867,4.133-4.15C45.399,20.425,43.548,18.557,41.267,18.557z");
            setUpPen();
            //addQuestionToPanel(new BattleQuestion(random));

            for (int i = 0; i < 3; i++)
            {
                createQuestion();
            }
        }  
        
        private void createQuestion()
        {
            var battleQuestion = new BattleQuestion(random);
            battleQuestions.Add(battleQuestion);
            addQuestionToPanel(battleQuestion);
        }

        private void addQuestionToPanel(BattleQuestion battleQuestion)
        {
            StackPanel stackPanel= new StackPanel();
            stackPanel.Orientation=Orientation.Horizontal;

            Button button = new Button();
            button.Style = (Style)Application.Current.Resources["BattleOption"];
            button.Content = battleQuestion.content;
            button.Height = 55;
            if (battleQuestion.isDamage == 1)
            {
                BattleOptionProperties.SetMyPathData(button, sword);
            }
            else
            {
                BattleOptionProperties.SetMyPathData(button, health);
            }

            TextBlock textBlock = new TextBlock();
            textBlock.Text = battleQuestion.value.ToString();
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.Margin = new Thickness(5, 10, 0, 0);
            textBlock.FontSize = 12;

            stackPanel.Children.Add(button);
            stackPanel.Children.Add(textBlock);
            stackPanel.RenderTransform=new TranslateTransform();
            questionsPanels.Add(stackPanel);
            battleQuestion.stackPanel = stackPanel;

            QuestionPanel.Children.Add(stackPanel);
        }

        private void AnswerBut_Click(object sender, RoutedEventArgs e)
        {
            string prediction= PredictionService.getPredictionFromInkCanvas(Drawing_Canvas);
            UserAnswer.Content = prediction;
            Drawing_Canvas.Strokes.Clear();
            checkQuestion(prediction);
        }

        private void checkQuestion(string prediction)
        {
            for(int i = 0; i < battleQuestions.Count; i++)
            {
                if (battleQuestions[i].ans.Equals(prediction))
                {
                    gameScore += battleQuestions[i].value * 100;
                    if (battleQuestions[i].isDamage == 1)
                    {
                        makeDamage(battleQuestions[i].value);
                    }
                    else
                    {
                        healUser(battleQuestions[i].value);
                    }
                    
                    animateGone(battleQuestions[i], () =>
                    {
                        
                        QuestionPanel.Children.Remove(battleQuestionToRemove.stackPanel);
                        battleQuestions.Remove(battleQuestionToRemove);
                        createQuestion();
                    });
                    break;
                }
            }
        }

        private void setHealth(int enemyHealth, int userHealth)
        {
            var width= enemyHealth * FullEnemyHealth.ActualWidth / 100;
            ActualEnemyHealth.Width = enemyHealth * FullEnemyHealth.ActualWidth / 100;
            ActualUserHealth.Width = userHealth * FullUserHealth.ActualWidth / 100;
        }

        private void makeDamage(int damage)
        {
            enemyHealth -= damage;
            if (enemyHealth < 0)
            {
                ((MainViewModel)DataContext).changeToBattleConclusionView.Execute(new BattleInfo(userHealth, gameScore));
            }
            else
            {

                ActualEnemyHealth.Width= enemyHealth*FullEnemyHealth.ActualWidth/100;
            }
        }

        private void takeDamage(int damage) {
            userHealth -= damage;
            if (userHealth < 0)
            {
                ((MainViewModel)DataContext).changeToBattleGameOverView.Execute(new BattleInfo(userHealth, gameScore));
            }
            else
            {

                ActualUserHealth.Width = userHealth * FullUserHealth.ActualWidth / 100;
            }
        }

        private void healUser(int heal)
        {
            userHealth += heal;
            if (userHealth > 100)
                userHealth = 100;
            ActualUserHealth.Width = userHealth * FullUserHealth.ActualWidth / 100;
        }

        private void animateGone(BattleQuestion battleQuestion, Action action= null)
        {
            StackPanel stackPanel = battleQuestion.stackPanel;
            DoubleAnimation doubleAnimation = new DoubleAnimation(-200, TimeSpan.FromSeconds(0.4));
            doubleAnimation.Completed += (sender, e) => action?.Invoke();
            stackPanel.RenderTransform.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
            battleQuestionToRemove = battleQuestion;
        }

        private void setUpPen()
        {
            var drawingAtributes = Drawing_Canvas.DefaultDrawingAttributes;

            drawingAtributes.Width = 5;
            drawingAtributes.Height = 5;
            drawingAtributes.FitToCurve = true;

            Drawing_Canvas.DefaultDrawingAttributes = drawingAtributes;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setHealth(userHealth, enemyHealth);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5); 
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (random.Next(10) > 6)
            {
                var damage = random.Next(2, 20);
                takeDamage(damage);
            }
        }
    }
}
