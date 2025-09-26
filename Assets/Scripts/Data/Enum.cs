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
    Equipment,
    Consumable,
    Quest,
    Material,
}

public enum ItemStatus
{
    Atk,
    Def,
    Crit,
    CritDmg,
}

public enum StatusType
{
    MaxHp,
    Hp,
    MaxMp,
    Mp,
    MaxStamina,
    Stamina,
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

public enum ConsumableType
{
    ResotreHp,
    ResotreMp,
    ResotreStamina,
}

public enum ConsumableBuffType
{
    Atk,
    Def,
}

public enum AI
{
    AI_CREATE,//����
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



