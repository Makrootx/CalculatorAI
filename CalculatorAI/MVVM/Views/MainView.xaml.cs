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
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void CaclulatorViewBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).changeToolbarIndex(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).changeToolbarIndex(2);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).changeToolbarIndex(3);
        }
    }
}
