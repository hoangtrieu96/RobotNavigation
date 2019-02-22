@name Robot Navigation Application
@author Trieu Hoang Nguyen
@description Apply searching algorithms with simple GUI

Application instructions

To open the application, go to the following folder: RobotNavigationGUI\bin\Debug

1. Run the application with RobotNavigationGUI.exe file
2. The first window is required 2 user inputs which is file name and search method
3. Enter appropriate inputs
4. The second screen will be displayed a map (with robot, goal, wall, path) and below the map will be the
result of the search process in words.
5. If you want to run another file or method, close the second window and enter different inputs in the first window.
So that means the first window is the main controller and do not need to close it.

- File name is the map file name with extension .txt
- The map must be put in the this folder (RobotNavigationGUI\bin\Debug)
- Method are: BFS (Breadth first search), DFS (Depth First Search), GBFS (Greedy Best First Search), AS (A*)
IDDFS (Iterative deepening Depth First Search), IDAS (Iterative deepening A*)

Note: The direction arrows in the map is a little bit confusing, for example the path is: up, right, down, down, right
Then the arrows will look like up, right, down, down and it starts from the second step and skip the last step
Which except the robot grid and the goal grid, these two grid with not display any arrows.
Therefore, if it confuses you, then look at the path in words at the bottom text box.

Note2: If the map is big such 10x10 or more, IDDFS and IDAS could take several minutes to complete.
Lastly, IDAS is limited with the boundary, therefore with a large map, in some cases it cannot reach to the goal.

Acknowledgements/Resources
PriorityQueue class from https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
Because C# does not have Priority Queue data structure, so that I need to install this Nuget package (Priority Queue.dll)
and it provides me an appropriate data structure for GBFS and A* search.