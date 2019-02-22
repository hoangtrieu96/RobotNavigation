using System;

namespace RobotNavigationGUI
{
    public static class GetPathBetweenTwoGrids
    {
        public static String ConnectTwoGrids(Grid fromGrid, Grid toGrid)
        {
            String path = "";
            //Get path by examine every single direction
            //UP
            if (toGrid.X == fromGrid.X && toGrid.Y == (fromGrid.Y - 1))
            {
                path = "Up";
            }
            //Left
            else if (toGrid.X == (fromGrid.X - 1) && toGrid.Y == fromGrid.Y)
            {
                path = "Left";
            }
            //Down
            else if (toGrid.X == fromGrid.X && toGrid.Y == (fromGrid.Y + 1))
            {
                path = "Down";
            }
            //Right
            else if (toGrid.X == (fromGrid.X + 1) && toGrid.Y == fromGrid.Y)
            {
                path = "Right";
            }

            return path;
        }
    }
}
