using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar_Pesi
{
    class PriorityQueue//minHeap
    {
        private Dictionary<Node, int> items;
        public bool IsEmpty { get { return items.Count == 0; } }

        public PriorityQueue()
        {
            items = new Dictionary<Node, int>();
        }

        public void Enqueue(Node node, int priority)
        {
            if(!items.ContainsKey(node))
                items.Add(node, priority);
        }
        public Node Dequeue()
        {
            int lowestPriority = int.MaxValue;
            Node selectedNode = null;
            foreach(Node currentNode in items.Keys)
            {
                int currentPriority = items[currentNode];
                if(currentPriority < lowestPriority)
                {
                    lowestPriority = currentPriority;
                    selectedNode = currentNode;
                }
            }
            items.Remove(selectedNode);
            return selectedNode;
        }
    }
}
