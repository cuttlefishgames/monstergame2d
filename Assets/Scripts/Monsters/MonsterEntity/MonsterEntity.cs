using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class MonsterEntity : MonoBehaviour, IMonster
    {
        [Header("Quick Test Variables")]
        public Elements Element;

        [SerializeField] protected MonsterStatsData _stats;
        [SerializeField] [Range(1, 100)] private int _level = 1;
        private string _GUID;

        private MonsterStats _updatedStats;
        private int _maxHP;
        private int _currentHP;
        private float _HPState;

        private void Awake()
        {
            SetGUID(System.Guid.NewGuid().ToString());
        }

        private MonsterAnimationController AnimationController
        {
            get
            {
                if (_animationController == null)
                {
                    _animationController = GetComponentInChildren<MonsterAnimationController>();

                    if (_animationController == null)
                    {
                        _animationController = gameObject.AddComponent<MonsterAnimationController>();
                    }

                    _animationController.AttachToMonster(this);
                }

                return _animationController;
            }
        }
        private MonsterAnimationController _animationController;

        private MonsterAnimationSpecialPoints AnimationPoints
        {
            get
            {
                if (_animationPoints == null)
                {
                    _animationPoints = GetComponentInChildren<MonsterAnimationSpecialPoints>();

                    if (_animationPoints == null)
                    {
                        _animationPoints = gameObject.AddComponent<MonsterAnimationSpecialPoints>();
                    }

                    _animationPoints.AttachToMonster(this);
                }

                return _animationPoints;
            }
        }
        private MonsterAnimationSpecialPoints _animationPoints;

        private MonsterEventsManager Events
        {
            get
            {
                if (_events == null)
                {
                    _events = GetComponentInChildren<MonsterEventsManager>();

                    if (_events == null)
                    {
                        _events = gameObject.AddComponent<MonsterEventsManager>();
                    }

                    _events.AttachToMonster(this);
                }

                return _events;
            }
        }
        private MonsterEventsManager _events;

        public Elements GetElement()
        {
            return Element;
        }

        public string GetGUID()
        {
            return _GUID;
        }

        public void SetGUID(string GUID)
        {
            _GUID = GUID;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public MonsterAnimationController GetAnimationController()
        {
            return AnimationController;
        }

        public MonsterAnimationSpecialPoints GetPointsManager()
        {
            return AnimationPoints;
        }

        public MonsterEventsManager GetMonsterEventsManager()
        {
            return Events;
        }

        public MonsterStatsData GetMonsterStats()
        {
            return _stats;
        }

        public void SetLevel(int level)
        {
            _level = level;
        }

        public int GetLevel()
        {
            return _level;
        }

        public void UpdateStats()
        {
            _updatedStats = StatsManager.UpdateStats(_stats, _level);
            _maxHP = _updatedStats.HP;
            _currentHP = _maxHP;
        }

        public float GetHPStatus()
        {
            return _currentHP / (float)_maxHP;
        }

        public void ReduceHP(int hp)
        {
            _currentHP = Mathf.Max(0, _currentHP - hp);
            Events.OnHPReduced(hp);

            if(_currentHP == 0)
            {
                AnimationController.SetState(MonsterAnimationStates.FAINT);
            }
        }
    }
}