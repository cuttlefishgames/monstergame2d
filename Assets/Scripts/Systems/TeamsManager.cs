using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public enum TeamSides { LEFT = 0, RIGHT = 100 }

    public class TeamsManager : Utils.Singleton<TeamsManager>
    {
        const int MAX_TEAM_SIZE = 4;

        private List<IMonster> _leftSide;
        private List<IMonster> _rightSide;

        protected override void Awake()
        {
            _leftSide = new List<IMonster>();
            _rightSide = new List<IMonster>();
        }

        public bool AddMonsterToTeam(IMonster mon, TeamSides side)
        {
            List<IMonster> team = new List<IMonster>();
            switch (side) 
            {
                case TeamSides.LEFT:
                    team = _leftSide;
                    break;
                case TeamSides.RIGHT:
                    team = _rightSide;
                    break;
            }

            if(team.Count >= MAX_TEAM_SIZE)
            {
                Debug.LogError("Trying to add a mon to a team that is already full");
                return false;
            }

            team.Add(mon);
            var position = BattlePositionsManager.Instance.GetNextFreePositionOfSide(side);
            position.AttachHolder(mon);

            return true;
        }
    }
}