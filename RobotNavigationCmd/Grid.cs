using System;
namespace RobotNavigation
{
	public class Grid
	{
		private bool _explored;
		private GridType _type;
        private int _x;
        private int _y;
        private Grid _parent;
        private String _pathLeadTo;
        private int _distanceToGoal;
        private int _currentStepCost;
        private bool _marked;

		public Grid(bool explored, GridType type, int x, int y)
		{
			_explored = explored;
			_type = type;
            _x = x;
            _y = y;
            _parent = null;
            _pathLeadTo = "";
            _distanceToGoal = 0;
            _currentStepCost = 0;
            _marked = false;
		}

		public bool Explored
		{
			get { return this._explored; }
			set { this._explored = value; }
		}

		public GridType Type
		{
			get { return this._type; }
			set { this._type = value; }
		}

        public int X
        {
            get { return this._x; }
        }

        public int Y
        {
            get { return this._y; }
        }

        public Grid Parent
        {
            get { return this._parent; }
            set { this._parent = value; }
        }

        public String PathLeadTo
        {
            get { return this._pathLeadTo; }
            set { this._pathLeadTo = value; }
        }

        public int DistanceToGoal
        {
            get { return this._distanceToGoal;  }
            set { this._distanceToGoal = value; }
        }

        public int CurrentStepCost
        {
            get { return this._currentStepCost; }
            set { this._currentStepCost = value; }
        }

        public bool Marked
        {
            get { return this._marked; }
            set { this._marked = value; }
        }
    }
}
