using System;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

namespace RobotNavigation
{
    public class Processor
    {
        private Robot _robot;
        private Grid[,] _map;
        private String _algorithm;
        private String _fileName;

        public Processor(Robot robot, Grid[,] map, String algorithm, String aFile)
        {
            _robot = robot;
            _map = map;
            _algorithm = algorithm;
            _fileName = aFile;
        }

        //Core function - determine which algorithm will be process
        public CombineObject FindGoal()
        {
            CombineObject result = new CombineObject(_map, "No solution found", 0);
            switch (_algorithm)
            {
                case "BFS":
                    result = ApplyBFS();
                    break;
                case "DFS":
                    result = ApplyDFS();
                    break;
                case "GBFS":
                    result = ApplyGBFS();
                    break;
                case "AS":
                    result = ApplyAS();
                    break;
                case "IDDFS":
                    result = ApplyIDDFS();
                    break;
                case "IDAS":
                    result = ApplyIDAS();
                    break;
                default:
                    _algorithm = "Unknown algorithm";
                    break;
            }
            Console.WriteLine("Filename: " + _fileName);
            Console.WriteLine("Method: " + _algorithm);
            Console.WriteLine("Number of nodes: " + result.NumberOfNodes);
            Console.WriteLine("The path: " + result.Path);
            return result;
        }


        /*
         * Search algorithm applying part
         */
        //Apply Breadth First Search with recursion prevention
        public CombineObject ApplyBFS()
        {
            //The final path and map to return in a CombineObject
            CombineObject result = new CombineObject(_map, "No solution found", 0);
            //Create a queue data structure for BFS
            Queue q = new Queue();
            //Add starting grid
            q.Enqueue(_map[_robot.Y, _robot.X]);

            //Loop until no more unexplored grid
            while (q.Count > 0)
            {
                Grid currentGrid;  //Current grid to explore
                currentGrid = (Grid)q.Dequeue(); //Remove from queue and start explore

                if (!currentGrid.Explored)
                {
                    currentGrid.Explored = true;   //Set it as explored

                    //Check if currentGrid is goal
                    if (currentGrid.Type == GridType.GOAL)
                    {
                        List<Grid> path = TraceBack(_map[_robot.Y, _robot.X], currentGrid);
                        //The best path it found
                        String aPath = "";
                        foreach (Grid g in path)
                        {
                            if (g != path[0])
                            {
                                aPath += g.PathLeadTo + "; ";
                            }
                        }
                        result.Path = aPath;
                        result.NumberOfNodes = GetNumberOfNodes();
                        return result;
                    }

                    //Get list of neighbour grids
                    List<Grid> neighbourGrids = NeighbourGridToMove(currentGrid);

                    //Explore each edge by the coordinates of currentGrid
                    //When all else is equal, move following this sequence: UP, LEFT, DOWN, RIGHT
                    if (neighbourGrids.Count > 0)
                    {
                        foreach (Grid g in neighbourGrids)
                        {
                            q.Enqueue(g);
                            g.Parent = currentGrid;
                        }
                    }
                }
            }
            result.NumberOfNodes = GetNumberOfNodes();
            return result;
        }

        //Apply Breadth First Search with recursion prevention
        public CombineObject ApplyDFS()
        {
            //The final path and map to return in a CombineObject
            CombineObject result = new CombineObject(_map, "No solution found", 0);
            //Create a stack data structure to store node grids
            Stack st = new Stack();
            //List to store the correct path to goal
            List<String> path = new List<string>();
            //Push the first node (robot initial location) to the stack
            st.Push(_map[_robot.Y, _robot.X]);
            //Set explored to true
            _map[_robot.Y, _robot.X].Explored = true;

            while (st.Count > 0)    //Do the loop when stack is not empty
            {
                Grid currentGrid = (Grid)st.Peek();

                //Check if the current grid is goal
                if (currentGrid.Type == GridType.GOAL)
                {
                    //The best path it found
                    String aPath = "";
                    foreach (String p in path)
                    {
                        aPath += p + "; ";
                    }
                    foreach (Grid g in st)
                    {
                        if (g.Type != GridType.ROBOT && g.Type != GridType.GOAL)
                        {
                            g.Type = GridType.PATH;
                        }
                    }
                    result.Path = aPath;
                    result.NumberOfNodes = GetNumberOfNodes();
                    return result;
                }

                //Get list of possible neighbour grids
                List<Grid> neighbourGrids = NeighbourGridToMove(currentGrid);

                //If there is one or more moves in neighbour grids then just push the first one
                if (neighbourGrids.Count > 0)
                {
                    neighbourGrids[0].Explored = true;
                    st.Push(neighbourGrids[0]);
                    path.Add(neighbourGrids[0].PathLeadTo);
                }
                //Else pop the top grid out of the stack to come back previous grid
                else
                {
                    if (path.Count > 0)
                        path.RemoveAt(path.Count - 1);
                    st.Pop();
                }
            }
            result.NumberOfNodes = GetNumberOfNodes();
            return result;
        }

        //Apply Greedy Best First Search with recursion prevention
        public CombineObject ApplyGBFS()
        {
            //The final path and map to return in a CombineObject
            CombineObject result = new CombineObject(_map, "No solution found", 0);
            //Create a priority queue data structure for GBFS
            SimplePriorityQueue<Grid> priorityQ = new SimplePriorityQueue<Grid>();
            //Add starting grid
            priorityQ.Enqueue(_map[_robot.Y, _robot.X], 1);

            while (priorityQ.Count > 0)
            {
                Grid currentGrid;  //Current grid to explore
                currentGrid = priorityQ.Dequeue(); //Remove from queue and start explore

                if (!currentGrid.Explored)
                {
                    currentGrid.Explored = true;   //Set it as explored

                    //Check if currentGrid is goal
                    if (currentGrid.Type == GridType.GOAL)
                    {
                        List<Grid> path = TraceBack(_map[_robot.Y, _robot.X], currentGrid);
                        //The best path it found
                        String aPath = "";
                        foreach (Grid g in path)
                        {
                            if (g != path[0])
                            {
                                aPath += g.PathLeadTo + "; ";
                            }
                        }
                        result.Path = aPath;
                        result.NumberOfNodes = GetNumberOfNodes();
                        return result;
                    }

                    //Get list of neighbour grids
                    List<Grid> neighbourGrids = NeighbourGridToMove(currentGrid);

                    //Explore each edge by the coordinates of currentGrid
                    //When all else is equal, move following this sequence: UP, LEFT, DOWN, RIGHT
                    if (neighbourGrids.Count > 0)
                    {
                        foreach (Grid g in neighbourGrids)
                        {
                            priorityQ.Enqueue(g, g.DistanceToGoal);
                            g.Parent = currentGrid;
                        }
                    }
                }
            }
            result.NumberOfNodes = GetNumberOfNodes();
            return result;
        }

        //Apply A* Search with recursion prevention
        public CombineObject ApplyAS()
        {
            //The final path and map to return in a CombineObject
            CombineObject result = new CombineObject(_map, "No solution found", 0);
            //Create a priority queue data structure for A*
            SimplePriorityQueue<Grid> priorityQ = new SimplePriorityQueue<Grid>();
            //Add starting grid
            priorityQ.Enqueue(_map[_robot.Y, _robot.X], 1);
            //Initial step cost value is 0 and increase by 1
            int stepCost = 0;

            while (priorityQ.Count > 0)
            {
                Grid currentGrid;  //Current grid to explore
                currentGrid = priorityQ.Dequeue(); //Remove from queue and start explore
                //Get the current step cost of current grid and assign to the stepCost that we are using
                //Do this to avoid stacking step cost when we stuck with a grid and dequeue to go back with the previous one or more steps
                stepCost = currentGrid.CurrentStepCost;

                if (!currentGrid.Explored)
                {
                    currentGrid.Explored = true;   //Set it as explored

                    //Check if currentGrid is goal
                    if (currentGrid.Type == GridType.GOAL)
                    {
                        List<Grid> path = TraceBack(_map[_robot.Y, _robot.X], currentGrid);
                        //The best path it found
                        String aPath = "";
                        foreach (Grid g in path)
                        {
                            if (g != path[0])
                            {
                                aPath += g.PathLeadTo + "; ";
                            }
                        }
                        result.Path = aPath;
                        result.NumberOfNodes = GetNumberOfNodes();
                        return result;
                    }

                    //Get list of neighbour grids
                    List<Grid> neighbourGrids = NeighbourGridToMove(currentGrid);

                    //Explore each edge by the coordinates of currentGrid
                    //When all else is equal, move following this sequence: UP, LEFT, DOWN, RIGHT
                    if (neighbourGrids.Count > 0)
                    {
                        stepCost++;
                        foreach (Grid g in neighbourGrids)
                        {
                            g.CurrentStepCost = stepCost;
                            //g.Distance and g.CurrentStepCost to goal equivalent to h(n) and g(n)
                            //Follow the formula: f(n) = g(n) + h(n)
                            priorityQ.Enqueue(g, g.DistanceToGoal + g.CurrentStepCost);
                            g.Parent = currentGrid;
                        }
                    }
                }
            }
            result.NumberOfNodes = GetNumberOfNodes();
            return result;
        }

        //Apply Iterative Deepening Depth First Search (IDDFS) with the implementation of DLS
        public CombineObject ApplyIDDFS()
        {
            //The final path and map to return in a CombineObject
            CombineObject result = new CombineObject(_map, "No solution found", 0);
            //The depth for each DLS loop
            int depth = 0;
            int nodesOfEveryEvenDepth = GetNumberOfNodes();
            int nodesOfEveryOddDepth = 0;
            Grid goalFound = null;
            while (nodesOfEveryEvenDepth != nodesOfEveryOddDepth)
            {
                goalFound = ApplyDLS(_map[_robot.Y, _robot.X], depth);
                if (goalFound != null)
                {
                    List<Grid> path = TraceBack(_map[_robot.Y, _robot.X], goalFound);
                    //The best path it found
                    String aPath = "";
                    foreach (Grid g in path)
                    {
                        if (g != path[0])
                        {
                            aPath += g.PathLeadTo + "; ";
                        }
                    }
                    result.Path = aPath;
                    result.NumberOfNodes = GetNumberOfNodes();
                    return result;
                }
                //Used to check if any new nodes was explored in the next depth
                //If not then terminate the loop
                if (depth % 2 == 0)
                {
                    nodesOfEveryEvenDepth = GetNumberOfNodes();
                }
                if (depth % 2 == 1)
                {
                    nodesOfEveryOddDepth = GetNumberOfNodes();
                }
                depth++;
            }
            result.NumberOfNodes = GetNumberOfNodes();
            return result;
        }

        //Apply Iterative Deepening A* Search (IDA*) with the implementation of BoundLimitedSearch
        public CombineObject ApplyIDAS()
        {
            //The final path and map to return in a CombineObject
            CombineObject result = new CombineObject(_map, "No solution found", 0);
            int bound = _map[_robot.Y, _robot.X].DistanceToGoal;
            int times = 0;

            while (times < 70)
            {
                IDAReturnObject aObject = ApplyBoundLimitedSearch(_map[_robot.Y, _robot.X], 0, bound);

                if (aObject.isGoalFound)
                {
                    List<Grid> path = TraceBack(_map[_robot.Y, _robot.X], aObject.Goal);
                    //The best path it found
                    String aPath = "";
                    foreach (Grid g in path)
                    {
                        if (g != path[0])
                        {
                            aPath += g.PathLeadTo + "; ";
                        }
                    }
                    result.Path = aPath;
                    result.NumberOfNodes = GetNumberOfNodes();
                    return result;
                }

                if (aObject.getBound == int.MaxValue)
                {
                    result.NumberOfNodes = GetNumberOfNodes();
                    return result;
                }
                bound = aObject.getBound;
                times++;
            }
            result.NumberOfNodes = GetNumberOfNodes();
            return result;
        }


        /*
         *  Utility methods for applying all algorithm
         */
        //Check if next move is not collide wall
        public bool HasWallsInNextMove(int nextX, int nextY)
        {
            bool result = false;
            if (_map[nextY, nextX].Type == GridType.WALL)
            {
                result = true;
            }
            return result;
        }

        //Check if next move is possible by check next move is still in the map and not collide walls
        public bool PossibleMove(int nextX, int nextY)
        {
            bool result = false;

            if ((nextX < 0) || (nextX >= _map.GetLength(1)))
            {
                return result;
            }
            if ((nextY < 0) || (nextY >= _map.GetLength(0)))
            {
                return result;
            }
            else
            {
                if (!HasWallsInNextMove(nextX, nextY))
                {
                    result = true;
                }
            }
            return result;
        }

        //Check neighbour grids in 4 directions and determine the grids that are possible to move to
        public List<Grid> NeighbourGridToMove(Grid currentGrid)
        {
            //A list to store possible neighbour girds
            List<Grid> neighBors = new List<Grid>();

            //Up neighbour grid
            if (PossibleMove(currentGrid.X, currentGrid.Y - 1) && !_map[currentGrid.Y - 1, currentGrid.X].Explored)
            {
                neighBors.Add(_map[currentGrid.Y - 1, currentGrid.X]);
                _map[currentGrid.Y - 1, currentGrid.X].PathLeadTo = "Up";
            }
            //Left neighbour grid
            if (PossibleMove(currentGrid.X - 1, currentGrid.Y) && !_map[currentGrid.Y, currentGrid.X - 1].Explored)
            {
                neighBors.Add(_map[currentGrid.Y, currentGrid.X - 1]);
                _map[currentGrid.Y, currentGrid.X - 1].PathLeadTo = "Left";
            }
            //Down neighbour grid
            if (PossibleMove(currentGrid.X, currentGrid.Y + 1) && !_map[currentGrid.Y + 1, currentGrid.X].Explored)
            {
                neighBors.Add(_map[currentGrid.Y + 1, currentGrid.X]);
                _map[currentGrid.Y + 1, currentGrid.X].PathLeadTo = "Down";
            }
            //Right neighbour grid
            if (PossibleMove(currentGrid.X + 1, currentGrid.Y) && !_map[currentGrid.Y, currentGrid.X + 1].Explored)
            {
                neighBors.Add(_map[currentGrid.Y, currentGrid.X + 1]);
                _map[currentGrid.Y, currentGrid.X + 1].PathLeadTo = "Right";
            }
            return neighBors;
        }

        //Trace back the path for BFS
        public List<Grid> TraceBack(Grid startGrid, Grid endGrid)
        {
            List<Grid> path = new List<Grid>();
            path.Add(endGrid);
            int i = 0;
            while (path[path.Count - 1] != startGrid)
            {
                path.Add(path[path.Count - 1].Parent);
                i++;
            }
            foreach (Grid g in path)
            {
                if (g.Type != GridType.ROBOT && g.Type != GridType.GOAL)
                {
                    g.Type = GridType.PATH;
                }
            }
            path.Reverse();
            return path;
        }

        public int GetNumberOfNodes()
        {
            int result = 0;
            foreach (Grid g in _map)
            {
                if (g.Explored || g.Marked)
                {
                    result++;
                }
            }
            return result;
        }

        //Depth limited depth first search (DLS) which is used for implement Iterative deepening DFS (IDDFS)
        public Grid ApplyDLS(Grid node, int depth)
        {
            if (depth == 0 && node.Type == GridType.GOAL)
            {
                return node;
            }
            if (depth > 0)
            {
                //Get list of neighbour grids (child node)
                List<Grid> neighbourGrids = NeighbourGridToMove(node);
                
                if (neighbourGrids.Count > 0)
                {
                    foreach (Grid g in neighbourGrids)
                    {
                        //Marked properties is for visualizer purpose and only apply for Iterative deepening searches
                        g.Marked = true;
                        Grid goalFound = ApplyDLS(g, depth - 1);
                        g.Parent = node;
                        g.PathLeadTo = GetPathBetweenTwoGrids.ConnectTwoGrids(node, g);
                        if (goalFound != null)
                        {
                            return goalFound;
                        }
                    }
                }
            }
            return null;
        }

        //Apply bound-limited search which is used for implement Iterative Deepening A* (IDA*)
        public IDAReturnObject ApplyBoundLimitedSearch(Grid node, int gn, int bound)
        {
            //f(n): the lowest cost path from current grid to node grid then to goal
            //h(n): cost from node to goal <=> node.DistanceToGoal
            //g(n): cost from current grid to node grid
            int fn = gn + node.DistanceToGoal;
            IDAReturnObject result = new IDAReturnObject();

            if (fn > bound)
            {
                result.getBound = fn;
                return result;
            }
            if (node.Type == GridType.GOAL)
            {
                result.isGoalFound = true;
                result.Goal = node;
                return result;
            }
            int min = int.MaxValue;
            //Get list of neighbour grids (child node)
            List<Grid> neighbourGrids = NeighbourGridToMove(node);

            if (neighbourGrids.Count > 0)
            {
                foreach (Grid g in neighbourGrids)
                {
                    //Marked properties is for visualizer purpose and only apply for Iterative deepening searches
                    g.Marked = true;
                    if (g != node.Parent)
                    {
                        IDAReturnObject newObject = ApplyBoundLimitedSearch(g, gn + g.DistanceToGoal, bound);
                        g.Parent = node;
                        g.PathLeadTo = GetPathBetweenTwoGrids.ConnectTwoGrids(node, g);
                        if (newObject.isGoalFound)
                            return newObject;
                        if (newObject.getBound < min)
                            min = newObject.getBound;
                    }
                }
            }
            result.getBound = min;
            return result;
        }
    }
}