using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RobotNavigationGUI
{
    /*
    * Setup Robot and Map by reading file and initialize with given data
    */
    public static class SetUpAndProcess
    {
        public static CombineObject InitializeAndProcess(String mapFile, String searchAlgorithm)
        {
            //Declare initial values
            int robotX = 0;
            int robotY = 0;
            int mapRows = 0;
            int mapCols = 0;
            int goalX = 0;
            int goalY = 0;
            List<int[]> wallList = new List<int[]>();

            //Declare variables for read file
            String formattedLine = "";
            String[] dataInString;
            int[] usableData;
            int count = 0;

            //Read file
            if (File.Exists(mapFile))
            {
                foreach (string line in File.ReadLines(mapFile))
                {
                    //Delete first and 2 last characters ([ and ]\n)
                    formattedLine = line.Substring(1, line.Length - 2);
                    //Now each line become x,y,w,h => split the ',' between and put each string number in an array
                    dataInString = formattedLine.Split(',');
                    //Convert the whole string array of numbers to int array
                    usableData = Array.ConvertAll(dataInString, int.Parse);

                    //Assign values here
                    switch (count)
                    {
                        case 0:
                            mapRows = usableData[0];
                            mapCols = usableData[1];
                            break;
                        case 1:
                            robotX = usableData[0];
                            robotY = usableData[1];
                            break;
                        case 2:
                            goalX = usableData[0];
                            goalY = usableData[1];
                            break;
                        default:
                            wallList.Add(usableData);
                            break;
                    }

                    //count use for assigning value (robot location, goal, then walls)
                    count++;
                }
            }
            else
            {
                MessageBox.Show("Error: File not found");
                System.Environment.Exit(1);
            }
            

            //Initialize robot and map
            Grid[,] aMap = new Grid[mapRows, mapCols];
            Robot aRobot = new Robot(robotX, robotY);

            /*
            //Initialize map with objects
            */
            //Initialize the map with ground type first
            for (int i = 0; i < mapRows; i++)
            {
                for (int j = 0; j < mapCols; j++)
                {
                    aMap[i, j] = new Grid(false, GridType.GROUND, j, i);
                }
            }

            //Set goal and robot in map
            //WARNING: aMap[m, n] = aMap[y, x] because m is row and n is col
            aMap[goalY, goalX].Type = GridType.GOAL;
            aMap[robotY, robotX].Type = GridType.ROBOT;
            //Marked true for robot because the initial grid is always the robot grid
            aMap[robotY, robotX].Marked = true;

            //Set up walls by x + width, y + height
            foreach (int[] wall in wallList)
            {
                for (int i = wall[0]; i < (wall[0] + wall[2]); i++)
                {
                    for (int j = wall[1]; j < (wall[1] + wall[3]); j++)
                    {
                        aMap[j, i].Type = GridType.WALL;
                    }
                }
            }

            //Calculate the Manhattan distance of each grid to the goal, except wall grids
            foreach (Grid g in aMap)
            {
                if (g.Type != GridType.WALL)
                {
                    g.DistanceToGoal = Math.Abs(g.X - goalX) + Math.Abs(g.Y - goalY);
                }
            }

            //Create Processor instance to deal with particular algorithm
            Processor mainProcessor = new Processor(aRobot, aMap, searchAlgorithm, mapFile);

            return mainProcessor.FindGoal();
        }
    }
}
