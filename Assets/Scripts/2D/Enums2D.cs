using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BattlefieldCharacterPositions 
{ 
    LEFT_TOP = 0,
    LEFT_MIDDLE = 1,
    LEFT_BOTTOM = 2,
    RIGHT_TOP = 3,
    RIGHT_MIDDLE = 4,
    RIGHT_BOTTOM = 5 
}

public enum BattlefieldPositions
{
    NONE = -1,
    MIDDLE = 0,
    MIDDLE_TOP = 1,
    MIDDLE_BOTTOM = 2,
    FRONT_MIDDLE_LEFT = 3,
    FRONT_MIDDLE_RIGHT = 4
}

public enum HopToPositionsTags
{
    STAY_IN_PLACE = 0,
    FRONT_OF_SELF = 1,
    BACK_OF_SELF = 2,
    MIDDLE = 3,
    MIDDLE_OF_TEAM = 4,
    MIDDLE_OF_ENEMY_TEAM = 5,
    FRONT_OF_TARGET = 6,
    BACK_OF_TARGET = 7
}

public enum TeamSides { LEFT = 0, RIGHT = 1 }

public enum Stats
{
    HP = 0,
    SPEED = 1,
    ATTACK = 2,
    MAGIC = 3,
    DEFENSE = 4,
    RESISTANCE = 5
}

public enum AnimationFixedPoints
{
    OVER = 0,
    FACE = 1,
    FRONT_BOTTOM = 2,
    FRONT_FACE = 3,
    CENTER = 4,
    BOTTOM = 5,
    TOP = 6,
    BACK = 7,
    BEHIND_BOTTOM = 8,
    BEHIND_CENTER = 9
}

public enum MovesIDs 
{
    NONE = -1,
    ONSLAUGHT = 0,
    TOPPLE = 1,
    FOREST_BLAST = 2,
    LASH = 3,
    LEAF_ARMOR = 4,
    DRAIN = 5,
    FLAME = 6
}

public enum Monster2DIDs
{
    NONE = -1,
    TSUCHIDRA = 0,
    HOOTROOT = 1
}



