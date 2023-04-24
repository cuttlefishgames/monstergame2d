using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class MonsterEventsManager : MonsterComponent
    {
        public delegate void OnHPReducedEvent(int hp);
        public OnHPReducedEvent HPReduced;
        public void OnHPReduced(int hp)
        {
            HPReduced?.Invoke(hp);
        }

        public delegate void OnDefeatedEvent();
        public OnDefeatedEvent Defeated;
        public void OnDefeated()
        {
            Defeated?.Invoke();
        }

        public delegate void OnVanishedEvent();
        public OnVanishedEvent Vanished;
        public void OnVanished()
        {
            Vanished?.Invoke();
        }
    }
}