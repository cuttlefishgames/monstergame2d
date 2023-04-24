using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.Spawner
{
    public class SpawnSpot : MonoBehaviour
    {
        [SerializeField] [Range(0, 50)] private int _roamingRange = 5;

        public bool Busy => Mon as UnityEngine.Object != null;
        public bool Free => !Busy;
        public bool Ready => Timer >= MaxTimer;
        public int Timer { get; private set; }
        public int MaxTimer { get; private set; }
        public IMonster Mon { get; private set; }

        //debug
        public int TimerDisplay;
        public int MaxTimerDisplay;

        public void SetMon(IMonster mon)
        {
            Mon = mon;
            Mon.GetMonsterEventsManager().Defeated += OnMonsterDefeated;
            Mon.GetMonsterEventsManager().Vanished += OnMonsterVanished;
            Mon.GetGameObject().transform.position = transform.position;

            var AIAgent = Mon.GetGameObject().GetComponent<Monster.AI.IAIAgent>();
            if(AIAgent as UnityEngine.Object != null)
            {
                AIAgent.SetRoamingRadius(_roamingRange);
                AIAgent.SetPivot(transform.position);
                AIAgent.SetState(AI.AIStates.ROAMING);                
            }
        }

        private void OnMonsterDefeated()
        {

        }

        private void OnMonsterVanished()
        {
            Mon.GetMonsterEventsManager().Defeated -= OnMonsterDefeated;
            Mon.GetMonsterEventsManager().Vanished -= OnMonsterVanished;
            ClearMon();
        }

        public void ClearMon()
        {
            Mon = null;
        }

        public void IncreaseTimer(int time = 1)
        {
            Timer += time;
            TimerDisplay = Timer;
        }

        public void SetTimer(int time)
        {
            Timer = time;
            TimerDisplay = Timer;
        }

        public void SetMaxTimer(int maxTime)
        {
            MaxTimer = maxTime;
            MaxTimerDisplay = MaxTimer;
        }
    }
}