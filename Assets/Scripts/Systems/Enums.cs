using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster 
{
    public enum ExpNeededCategories { 
        EASIEST = 0, VERY_EASY = 10, EASY = 20, AVERAGE = 30, A_LITTLE_SLOW = 40, 
        SLOW = 50, VERY_SLOW = 60, HARD = 70, VERY_HARD = 80, HARDEST = 90 }

    public enum Elements { PURE = 0, LIGHT = 100, NATURE = 200, SKY = 300, ACQUA = 400, TERRA = 500, PYRO = 600,
                           CRYO = 700, SHADOW = 800, CORRUPTION = 900, VOID = 1000 }

    public enum DamageTypes { PHYSICAL = 0, MAGICAL = 100 }
    public enum Stats { ATTACK = 100, MAGIC = 200, HP = 300, SPEED = 400, DEFENSE = 500, RESISTANCE = 600 }
}