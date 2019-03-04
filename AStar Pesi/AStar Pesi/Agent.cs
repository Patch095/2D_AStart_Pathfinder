using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Aiv.Fast2D;
using OpenTK;

namespace AStar_Pesi
{
    class Agent
    {
        protected Sprite sprite;
        protected Vector4 color;

        public int X { get { return Convert.ToInt32(sprite.position.X); }}
        public int Y { get { return Convert.ToInt32(sprite.position.Y); } }
        public Vector2 CurrentPosition { get { return new Vector2(X, Y); } }

        protected Node currNode;
        protected List<Node> path;
        public Node Target;
        protected float speedBost;

        public Agent(int x, int y, int speed)
        {
            sprite = new Sprite(1, 1)
            {
                position = new Vector2(x, y)
            };
            Target = null;

            color = new Vector4(0, 0, 255, 1);

            speedBost = speed;
        }

        public void SetPath(List<Node> p)
        {
            //You set the list of nodes he needs to follow
            path = p;
            if(Target == null && path.Count > 0)
            {
                Target = path[0];
                path.RemoveAt(0);
            }
            else if(path.Count > 0)
            {
                int dist = Program.Map.Heuristic(path[0], Target);
                if (dist > 1)
                {
                    path.Insert(0, currNode);
                }
            }
        }

        public virtual void Update()
        {
            //Using the update method he follows the path
            if(Target != null)
            {
                Vector2 destination = new Vector2(Target.X, Target.Y);
                Vector2 dir = destination - sprite.position;
                float distance = dir.Length;
                if (distance < 0.03f)
                {
                    currNode = Target;
                    sprite.position = destination;
                    if (path.Count > 0)
                    {
                        Target = path[0];
                        path.RemoveAt(0);
                    }
                    else
                        Target = null;
                }
                else
                    sprite.position += (dir.Normalized() * speedBost) * Program.Window.deltaTime;
            }
        }

        public virtual void Draw()
        {        
            //A blue box
            sprite.DrawSolidColor(color);
        }
    }
}
