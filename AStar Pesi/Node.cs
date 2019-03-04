using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace AStar_Pesi
{
    class Node
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public List<Node> Neighbours { get; private set; }
        public int Cost { get; set;}

        public Node(int x, int y, int cost)
        {
            X = x;
            Y = y;
            Cost = cost;
            Neighbours = new List<Node>();
        }

        public void AddNeighbour(Node node)
        {
            Neighbours.Add(node);
        }

        public void RemoveNeighbour(Node node)
        {
            for(int i = node.Neighbours.Count - 1; i >= 0; i--)
            {
                Node n = node.Neighbours[i];
                if(n != null)
                {
                    n.Neighbours.Remove(node);
                    node.Neighbours.Remove(n);
                }
            }
        }
    }
}
