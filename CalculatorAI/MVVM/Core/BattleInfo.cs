using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorAI.MVVM.Core
{
    public class BattleInfo
    {
        public int health;
        public int score;
        public BattleInfo() { 
            health = 100; 
            score = 0;
        }

        public BattleInfo(int health, int score)
        {
            this.health = health;
            this.score = score;
        }
    }
}
