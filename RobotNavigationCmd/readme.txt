@name Robot Navigation Application
@author Trieu Hoang Nguyen
@description Apply searching algorithms

Application instructions

This application is supposed to be run from command line with the following syntax:
search <filename> <method>

- Whereas filename is the map file name with extension .txt
- The map must be put in the this folder (or the same folder with search.exe)
- Method are: BFS (Breadth first search), DFS (Depth First Search), GBFS (Greedy Best First Search), AS (A*)
IDDFS (Iterative deepening Depth First Search), IDAS (Iterative deepening A*)

Note: the cs files is just for marking purpose which the examiner can look at the code and the Priority Queue.dll is
a additional package that is used when coding. Everything has been compiled in to search.exe, so any other files is
not very important.

Note2: If the map is big such 10x10 or more, IDDFS and IDAS could take several minutes to complete.
Lastly, IDAS is limited with the boundary, therefore with a large map, in some cases it cannot reach to the goal.

Acknowledgements/Resources
PriorityQueue class from https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
Because C# does not have Priority Queue data structure, so that I need to install this Nuget package (Priority Queue.dll)
and it provides me an appropriate data structure for GBFS and A* search.