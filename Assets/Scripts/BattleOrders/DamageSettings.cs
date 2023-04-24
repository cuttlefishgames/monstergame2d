using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    [System.Serializable]
    public class DamageSettings
    {
        [SerializeField] private float _multiplierPerHit = 100;
        [SerializeField] private DamageTypes _damageType = DamageTypes.PHYSICAL;
        [SerializeField] private Stats _offensiveStat = Stats.ATTACK;
        [SerializeField] private bool _multiHits = false;

        public float MultiplierPerHit { get => _multiplierPerHit; }
        public DamageTypes DamageType { get => _damageType; }
        public Stats OffensiveStat { get => _offensiveStat; }
        public bool MultiHits { get => _multiHits; }
    }
}