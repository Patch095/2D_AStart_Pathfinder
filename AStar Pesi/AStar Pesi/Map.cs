using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using System.IO;

namespace AStar_Pesi
{
    class Map
    {
        private Dictionary<Node, Node> cameFrom;
        private Dictionary<Node, int> costSoFar;
        private PriorityQueue frontier;

        private int width;
        private int heigth;
        private int[] cells;
        public Node[] Nodes { get; private set; }

        private Sprite sprite;

        public Map(int w, int h, int[] c)
        {
            width = w;
            heigth = h;
            cells = c;

            sprite = new Sprite(1, 1);
            Nodes = new Node[c.Length];

            //Build nodes for the cells
            for (int i = 0; i < cells.Length; i++)
            {
                int x = i % width;
                int y = i / width;
                Nodes[i] = new Node(x, y, cells[i]);
            }
            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    CheckNeighbours(Nodes[index], x, y - 1);//Check top neighbourg
                    CheckNeighbours(Nodes[index], x + 1, y);//Check rigth neighbourg
                    CheckNeighbours(Nodes[index], x, y + 1);//Check bot neighbourg
                    CheckNeighbours(Nodes[index], x - 1, y);//Check left neighbourg
                }
            }
        }

        private void AddNode(int x, int y , int cost = 1)
        {
            int index = y * width + x;
            Node node = new Node(x, y, cost);
            Nodes[index] = node;
            CheckNeighbours(Nodes[index], x, y);
            foreach(Node adj in node.Neighbours)
            {
                adj.AddNeighbour(node);
            }
        }
        private void RemoveNode(int x, int y)
        {
            int index = y * width + x;
            Node node = GetNode(x, y);
            foreach(Node adj in node.Neighbours)
            {
                adj.RemoveNeighbour(node);
            }
            cells[index] = 0;
        }
        public void ToggleNode(int x, int y)
        {
            Node node = GetNode(x, y);
            if(node == null)
            {
                AddNode(x, y);
            }
            else
            {
                RemoveNode(x, y);
            }
        }

        private void CheckNeighbours(Node currentNode, int cellX, int cellY)
        {
            if (cellX < 0 || cellX >= width || cellY < 0 || cellY >= heigth)
                return;
            int index = cellY * width + cellX;
            Node neighbours = Nodes[index];
            if (neighbours != null && !currentNode.Neighbours.Contains(neighbours))
            {
                currentNode.AddNeighbour(neighbours);
                neighbours.AddNeighbour(currentNode);
            }
        }

        public int Heuristic(Node start, Node end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }
        public void AStar(Node start, Node end)
        {
            cameFrom = new Dictionary<Node, Node>();
            costSoFar = new Dictionary<Node, int>();
            frontier = new PriorityQueue();
            costSoFar[start] = 0;
            cameFrom[start] = start;
            frontier.Enqueue(start, Heuristic(start, end));
            while (!frontier.IsEmpty)
            {
                Node currentNode = frontier.Dequeue();
                if (currentNode == end)
                    break;

                foreach (Node nextNode in currentNode.Neighbours)
                {
                    if (nextNode.Cost == 0)
                        continue;

                    int newCost = costSoFar[currentNode] + nextNode.Cost;
                    if (!costSoFar.ContainsKey(nextNode) || costSoFar[nextNode] > newCost)
                    {
                        costSoFar[nextNode] = newCost;
                        int priority = newCost + Heuristic(nextNode, end);
                        frontier.Enqueue(nextNode, priority);
                        cameFrom[nextNode] = currentNode;
                    }
                }
            }
        }

        public List<Node> GetPath(int startX, int startY, int endX, int endY)
        {
            //Get startNode and endNode
            List<Node> path = new List<Node>();

            Node startNode = GetNode(startX, startY);
            Node endNode = GetNode(endX, endY);
            if(startNode == null || endNode == null || startNode == endNode)
            {
                return null;
            }

            //Call A*
            if (startNode.Neighbours.Count == 0)
                return null;

            else
            {
                AStar(startNode, endNode);
                if (!cameFrom.ContainsKey(endNode))
                    return null;

                //Build the list of nodes from cameFrom dictionary
                Node currentNode = endNode;
                while (cameFrom[currentNode] != currentNode)
                {
                    path.Add(currentNode);
                    currentNode = cameFrom[currentNode];
                }

                //Return the list of nodes in the right order
                path.Reverse();

                return path;
            }
        }
        public Node GetNode(int x, int y)
        {
            if(x >= width || x < 0)
            {
                return null;
            }
            if (y >= heigth || y < 0)
            {
                return null;
            }
            int index = y * width + x;
            return Nodes[index];
        }

        public void ChangeNode(int x, int y)
        {
            int index = y * width + x;

            if(cells[index] > 0)
                cells[index] = 0;
            else
                cells[index] = 1;

            Nodes[index].Cost = cells[index];
        }

        public void Draw()
        {
            //Draw the map
            for(int i = 0; i < Nodes.Length; i++)
            {
                sprite.position = new Vector2(Nodes[i].X, Nodes[i].Y);
                if(Nodes[i].Cost == 0)
                {
                    sprite.DrawSolidColor(new Vector4(0, 0, 0, 1));
                }
                else
                {
                    sprite.DrawSolidColor(new Vector4(1, 1, 1, 1));
                }
            }
        }
    }
}
