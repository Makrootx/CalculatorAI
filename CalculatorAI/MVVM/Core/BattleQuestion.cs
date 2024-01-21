using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CalculatorAI.MVVM.Core
{
    public class BattleQuestion
    {
        public string content;
        public string ans;
        private Random random;
        public int value;
        public int isDamage;
        public StackPanel stackPanel;

        public BattleQuestion(Random random)
        {
            this.random = random;
            generateQuestion();
            isDamage = random.Next(2);
        }

        private void generateQuestion()
        {
            int a, b;
            if (random.Next(10) > 7)
            {
                a = random.Next(10, 100);
                b = random.Next(10, 100);
                value = random.Next(14, 24);
            }
            else
            {
                a = random.Next(0, 10);
                b = random.Next(0, 10);
                value = random.Next(6, 14);
            }
            if (random.Next(10) > 7)
            {
                setFields(new int[] { a, b }, '*');
            }
            else
            {
                if (random.Next(10) > 5)
                {
                    setFields(new int[] { a, b }, '-');
                }
                else
                {
                    setFields(new int[] { a, b }, '+');
                }
            }
        }

        private void setFields(int[] a, char operation)
        {
            int ans = 0;
            switch (operation)
            {
                case '*':
                    ans = a[0] * a[1];
                    content = a[0].ToString() + " " + "*" + " " + a[1].ToString();
                    this.ans = ans.ToString();
                    break;
                case '+':
                    ans = a[0] + a[1];
                    content = a[0].ToString() + " " + "+" + " " + a[1].ToString();
                    this.ans = ans.ToString();
                    break;
                    case '-':
                    ans = a[0] - a[1];
                    content = a[0].ToString() + " " + "-" + " " + a[1].ToString();
                    this.ans = ans.ToString();
                    break;
            }
            
        }
    }
}
