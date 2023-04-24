using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Monster
{
    public enum BattleFieldSides { LEFT = 0, RIGHT = 10 }
    public enum BattleFieldPositions { FIRST = 0, SECOND = 10, THIRD = 20, FOURTH = 30, MIDDLE = 40 }

    public class BattlePositionsManager : Utils.Singleton<BattlePositionsManager>
    {
        [Serializable]
        public class PositionData
        {
            public BattleFieldPositions BattleFieldPosition = BattleFieldPositions.FIRST;
            public Transform Position;
        }

        [Serializable]
        public class PointAnimationData
        {
            public AnimationPoints Point = AnimationPoints.CENTER;
            public bool FollowPoint = true;
        }


        public class PositionState
        {
            public PositionData PosData;
            public IMonster Holder;

            public bool IsFree => Holder == null && Holder as UnityEngine.Object == null;

            public void Clear()
            {
                Holder = null;
            }

            public void AttachHolder(IMonster mon)
            {
                Clear();
                Holder = mon;
            }
        }

        public Transform MiddlePosition { get => _battlefieldMiddle; }
        [SerializeField] private Transform _battlefieldMiddle;

        [SerializeField] public List<PositionData> _leftSide = new List<PositionData>
        {
            new PositionData { BattleFieldPosition = BattleFieldPositions.FIRST },
            new PositionData { BattleFieldPosition = BattleFieldPositions.SECOND },
            new PositionData { BattleFieldPosition = BattleFieldPositions.THIRD },
            new PositionData { BattleFieldPosition = BattleFieldPositions.FOURTH },
            new PositionData { BattleFieldPosition = BattleFieldPositions.MIDDLE }
        };

        [SerializeField]
        public List<PositionData> _rightSide = new List<PositionData>
        {
            new PositionData { BattleFieldPosition = BattleFieldPositions.FIRST },
            new PositionData { BattleFieldPosition = BattleFieldPositions.SECOND },
            new PositionData { BattleFieldPosition = BattleFieldPositions.THIRD },
            new PositionData { BattleFieldPosition = BattleFieldPositions.FOURTH },
            new PositionData { BattleFieldPosition = BattleFieldPositions.MIDDLE }
        };

        public List<PositionData> LeftSide { get => _leftSide; }
        public List<PositionData> RightSide { get => _rightSide; }

        public List<PositionState> LeftSideStates { get; private set; } = new List<PositionState>();
        public List<PositionState> RightSideStates { get; private set; } = new List<PositionState>();

        protected override void Awake()
        {
            base.Awake();
            LeftSideStates = new List<PositionState>();
            foreach(var posData in _leftSide)
            {
                LeftSideStates.Add(new PositionState()
                {
                    PosData = posData,
                    Holder = null
                });
            }

            RightSideStates = new List<PositionState>();
            foreach (var posData in _rightSide)
            {
                RightSideStates.Add(new PositionState()
                {
                    PosData = posData,
                    Holder = null
                });
            }
        }

        public PositionState GetNextFreePositionOfSide(TeamSides side)
        {
            PositionState positionState = null;
            switch (side)
            {
                case TeamSides.LEFT:
                    positionState = LeftSideStates.First(p => p.IsFree);
                    break;
                case TeamSides.RIGHT:
                    positionState = RightSideStates.First(p => p.IsFree);
                    break;
            }

            return positionState;
        }

    }
}