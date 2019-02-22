using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class IDAReturnObject
    {
        private int _bound;
        private Grid _goal;
        private bool _foundGoal;

        public IDAReturnObject()
        {
            _bound = int.MaxValue;
            _goal = null;
            _foundGoal = false;
        }

        public int getBound
        {
            get { return this._bound; }
            set { this._bound = value; }
        }

        public Grid Goal
        {
            get { return this._goal; }
            set { this._goal = value; }
        }

        public bool isGoalFound
        {
            get { return this._foundGoal; }
            set { this._foundGoal = value; }
        }
    }
}
