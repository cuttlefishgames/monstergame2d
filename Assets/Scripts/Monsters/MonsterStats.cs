using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    [System.Serializable]
    public class MonsterStats
    {
        public int Level { get; private set; }
        public int HP { get; private set; }
        public int Attack { get; private set; }
        public int Magic { get; private set; }
        public int Speed { get; private set; }
        public int Defense { get; private set; }
        public int Resistance { get; private set; }

        public void SetLevel(int level)
        {
            Level = level;
        }

        public void SetHP(int hp)
        {
            HP = hp;
        }

        public void SetAttack(int attack)
        {
            Attack = attack;
        }

        public void SetMagic(int magic)
        {
            Magic = magic;
        }

        public void SetSpeed(int speed)
        {
            Speed = speed;
        }

        public void SetDefense(int defense)
        {
            Defense = defense;
        }

        public void SetResistance(int resistance)
        {
            Resistance = resistance;
        }
    }
}