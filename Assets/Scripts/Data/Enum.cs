//씬
public enum SCENE
{
    TITLE,
    LOADING,
    MAIN,   
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

//아이템
public enum ItemType
{
    Equipment,
    Consumable,
    Quest,
    Material,
}

public enum ItemGrade
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4,
}

//소비아이템 
public enum ConsumableType
{
    ResotreHp,
    ResotreMp,
    ResotreStamina,
}

//장비아이템 
public enum EquipmentType
{
    Weapon,
    Head,
    Armor,
    Glove,
    Shoes,    
}

public enum ItemStatus
{
    Atk,
    Def,
    Crit,
    CritDmg,
}

//캐릭터 스테이터스 타입
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

//캐릭터 상태
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

public enum ConsumableBuffType
{
    Atk,
    Def,
}

public enum AI
{
    AI_CREATE,//생성
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

//퀘스트
public enum QuestClass
{
    Main = 0,
    Sub = 1,
    Repeat = 2,
    Daily = 3,
    Event = 4,
}
public enum QuestClassification
{
    NpcTolk,
    Kill,
    Collect,
}

public enum QuestCondition
{
    NotStart,
    Precess,
    Completed,
    Failed,
}





