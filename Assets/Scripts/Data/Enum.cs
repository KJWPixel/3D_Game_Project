public enum SCENE
{
    TITLE,
    LOBBY,
    MAIN,
    LOADING,
    ENG,
}

public enum MENU
{
    OPTION,
}

public enum SKILL
{
    ATTACK,
    SKILL1,
    END,
}

public enum ItemType
{
    None,
    Equipment,
    Consumable,
    Quest,
    Material,
}

public enum ItemStatus
{
    None,
    Atk,
    Def,
    Crit,
    CritDmg,
}

public enum CharacterStatus
{
    None,
    MaxHp,
    MaxMp,
    MaxStamina,
    Atk,
    Def,
    Crit,
    CritDmg,
}

public enum PlayerState
{
    Create,
    Idle,
    Walking,
    Running,
    Dashing,
    Attacking,
    Casting,
    Stunned,
}

public enum ConsumableEffect
{
    None,
    ResotreHp,
    ResotreMp,
    ResotreStamina,
}

public enum AI
{
    AI_CREATE,//»ý¼º
    AI_IDLE,  
    AI_SEARCH,
    AI_PATROL,
    AI_CHASE,
    AI_FLEE,
    AI_ATTACK,
    AI_SKILL,
    AI_DEAD,
    AI_RESET,
}



