using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace AStar_Pesi
{
    class Enemy : Agent
    {
        public Enemy(int x, int y, Vector4 c, int r) : base(x, y, r/10)
        {
            color = c;
        }
    }
}
