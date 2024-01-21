using CalculatorAI.MVVM.Core;
using CalculatorAI.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorAI.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для BattleGameOverScreen.xaml
    /// </summary>
    public partial class BattleGameOverScreen : UserControl
    {
        public BattleGameOverScreen(BattleInfo battleInfo)
        {
            InitializeComponent();
            Score.Text = battleInfo.score.ToString();
        }

        private void RetryBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).changeToBattleView.Execute(null);
        }
        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).changeToolbarIndex(2);
        }
    }
}
