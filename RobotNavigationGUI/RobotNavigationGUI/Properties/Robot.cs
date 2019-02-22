using System;
namespace RobotNavigationGUI
{
	public class Robot
	{
		private int _x;
		private int _y;

		public Robot(int x, int y)
		{
			_x = x;
			_y = y;
		}

		public int X
		{
			get { return this._x; }
			set { this._x = value; }
		}

		public int Y
        {
            get { return this._y; }
            set { this._y = value; }
        }
	}
}
