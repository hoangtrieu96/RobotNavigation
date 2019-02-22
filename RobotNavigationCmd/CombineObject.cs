using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    //This class is used to return an object that contains both map and the path
    public class CombineObject
    {
        private Grid[,] _map;
        private String _path;
        private int _numberOfNodes;

        public CombineObject(Grid[,] map, String path, int nodes)
        {
            _map = map;
            _path = path;
            _numberOfNodes = nodes;
        }

        public Grid[,] Map
        {
            get { return this._map; }
            set { this._map = value; }
        }

        public String Path
        {
            get { return this._path; }
            set { this._path = value; }
        }

        public int NumberOfNodes
        {
            get { return this._numberOfNodes; }
            set { this._numberOfNodes = value; }
        }
    }
}
