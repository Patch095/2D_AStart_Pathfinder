using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace AStar_Pesi
{
    class Program
    {
        static public Window Window;
        static private float timer;
        static private float defaultTimer = 2.0f;

        static public Map Map;

        static public List<Agent> Players;

        static Node currentDestination;

        static void Main(string[] args)
        {
            Window = new Window(700, 700, "A*");
            Window.SetDefaultOrthographicSize(10);

            Players = new List<Agent>();

            int[] cells = new int[]
            {
                //10,10
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 1, 1, 1, 0, 1, 1, 0, 1, 0,
                0, 1, 0, 1, 0, 0, 1, 0, 0, 0,
                0, 1, 0, 1, 1, 1, 1, 1, 1, 0,
                0, 1, 0, 1, 0, 0, 1, 0, 1, 0,
                0, 0, 0, 1, 0, 1, 1, 1, 1, 0,
                0, 0, 1, 1, 1, 0, 1, 0, 1, 0,
                0, 1, 0, 0, 1, 0, 1, 0, 1, 0,
                0, 1, 1, 1, 1, 0, 1, 1, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };
            Map = new Map(10, 10, cells);
            Agent agent = new Agent(1, 4, 2);
            Players.Add(agent);
            Random r = new Random();
            Players.Add(new Enemy(8, 7, new Vector4(255, 0, 0, 1), r.Next(10, 18)));
            Players.Add(new Enemy(1, 7, new Vector4(255, 0, 255, 1), r.Next(10, 18)));
            Players.Add(new Enemy(8, 1, new Vector4(0, 255, 0, 1), r.Next(10, 18)));

            bool clickedL = false;
            bool clickedR = false;
            timer = defaultTimer;

            while (Window.IsOpened)
            {
                if (Window.mouseLeft)
                {
                    if (!clickedL)
                    {
                        currentDestination = Map.GetNode((int)Window.mousePosition.X, (int)Window.mousePosition.Y);
                        clickedL = true;
                        if (currentDestination == null)
                            continue;
                        else
                        {
                            List<Node> path = Map.GetPath(agent.X, agent.Y, currentDestination.X, currentDestination.Y);
                            if (path != null && path.Count > 0)
                                agent.SetPath(path);
                        }
                    }
                }
                else if (clickedL)
                    clickedL = false;

                if (Window.mouseRight)
                {
                    if (!clickedR)
                    {
                        Map.ChangeNode((int)Window.mousePosition.X, (int)Window.mousePosition.Y);
                        for (int i = 0; i < Players.Count; i++)
                        {
                            Players[i].Target = null;
                            if (Players[i] is Enemy)
                            {
                                List<Node> chasingPath = Map.GetPath(Players[i].X, Players[i].Y, agent.X, agent.Y);
                                if (chasingPath != null && chasingPath.Count > 0)
                                    Players[i].SetPath(chasingPath);
                            }
                            else
                            {
                                List<Node> path = Map.GetPath(agent.X, agent.Y, currentDestination.X, currentDestination.Y);
                                if (path != null && path.Count > 0)
                                    agent.SetPath(path);
                            }
                        }
                        timer = defaultTimer;
                        //ricalcolo il path degli agent e dei nemici
                        clickedR = true;
                    }
                }
                else if (clickedR)
                    clickedR = false;

                timer -= Window.deltaTime;
                if (timer <= 0)
                {
                    for (int i = 0; i < Players.Count; i++)
                    {
                        if (Players[i] is Enemy)
                        {
                            List<Node> chasingPath = Map.GetPath(Players[i].X, Players[i].Y, agent.X, agent.Y);
                            if (chasingPath != null && chasingPath.Count > 0)
                                Players[i].SetPath(chasingPath);
                        }
                    }
                }

                for (int i = 0; i < Players.Count; i++)
                    Players[i].Update();

                Map.Draw();
                for (int i = 0; i < Players.Count; i++)
                    Players[i].Draw();

                Window.Update();
            }
        }
    }

}
