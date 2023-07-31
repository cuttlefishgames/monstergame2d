using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleStates 
{ 
    INACTIVE = 0,
    FILLING_BARS = 1, 
    BATTLE_MENU = 2, 
    MOVES_MENU = 3,
    TARGET_SELECTION = 4,
    RESOLVING_EVENTS = 5
}

public enum CharacterSlotStates
{
    IDLE = 0,
    HIGHLIGHT = 1,
    GREYED_OUT = 2,
    HIDDEN = 3
}

public enum BattlefieldCharacterPositions 
{ 
    LEFT_TOP = 0,
    LEFT_MIDDLE_1 = 1,
    LEFT_MIDDLE_2 = 6,
    LEFT_BOTTOM = 2,
    RIGHT_TOP = 3,
    RIGHT_MIDDLE_1 = 4,
    RIGHT_MIDDLE_2 = 7,
    RIGHT_BOTTOM = 5 
}

public enum BattlefieldPositions
{
    NONE = -1,
    MIDDLE = 0,
    MIDDLE_TOP = 1,
    MIDDLE_BOTTOM = 2,
    FRONT_MIDDLE_LEFT_1 = 3,
    FRONT_MIDDLE_LEFT_2 = 5,
    FRONT_MIDDLE_RIGHT_1 = 4,
    FRONT_MIDDLE_RIGHT_2 = 6
}

public enum AnimationStates
{
    IDLE = 0,
    WALK = 1,
    FREEZE = 2,
    HURT = 3
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

public enum Sizes
{
    TINY = 0,
    SMALL = 1,
    MEDIUM = 2,
    LARGE = 3,
    GIANT = 4,
    TITAN = 5,
    GODZILLA = 6
}

public enum MovesTargets 
{
    SELF = 0,
    ALL_ALLIES = 1,
    ALL_ALLIES_MINUS_SELF = 2,
    ALL = 3,
    ALL_MINUS_SELF = 4,
    ENEMY = 5,
    ALL_ENEMIES = 6
}

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
    FLAME = 6,
    BITE = 7,
    STAB = 8,
    PECK = 9,
    SCRATCH = 10,
    CHARGE = 11,
    STEAMROLLER = 12,
    MAGMA_LASH = 13,
    INCISIVE_CRUNCH = 14,
    BURROW = 15,
    TUNNEL_ATTACK = 16,
    SCREECH = 17, 
    SAND_BLAST = 18,
    SAND_TORNADO = 19, 
    LEAF_SWORD = 20,
    ROCK_SHOT = 21,
    EMBER_WIND = 22
}

public enum Monster2DIDs
{
    NONE = -1,
    TSUCHIDRA = 0,
    HOOTROOT = 1
}



