using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private PlayerSkillBook skillBook;
    [SerializeField] private Transform CurrentTrsSave;
    PlayerSaveData playerSaveData;
    string filePath;

    [Header("Quick Slots")]
    [SerializeField] private List<UI_ItemSlot> quickSlotList; 

    [System.Serializable]
    public class PlayerSaveData
    {
        //기본 정보 및 스탯
        public string UserName;
        public int Level;
        public float CurrentExp;
        public float MaxExp;
        public int SkillPoint;
        public int Gold;

        public float CurrentHp;
        public float MaxHp;
        public float CurrentMp;
        public float MaxMp;
        public float CurrentStamina;
        public float MaxStamina;

        public float Atk;
        public float Def;
        public float Critical;
        public float CritDmg;

        public float PosX;
        public float PosY;
        public float PosZ;

        public List<ItemSaveData> InventoryItems = new List<ItemSaveData>();
        public List<QuickSlotSaveData> QuickSlots = new List<QuickSlotSaveData>();  
        public List<SkillSaveData> LearnedSkills = new List<SkillSaveData>();
    }

    [System.Serializable]
    public class ItemSaveData
    {
        public int id;
        public int quantity;
        public bool isEquipped;
    }

    [System.Serializable]
    public class QuickSlotSaveData
    {
        public int slotIndex;
        public int ItemId;
    }

    [System.Serializable]
    public class SkillSaveData
    {
        public int id;
        public int level;
    }

    private void Awake()
    {
        filePath = Path.Combine(Application.dataPath, "playerData.json");
    }

    private void Start()
    {
        LoadGame();
    }


    public void SaveGame()
    {
        // 플레이어 정보 및 스탯 저장
        PlayerSaveData playerData = new PlayerSaveData();

        playerData.UserName = playerStat.UserName;
        playerData.Level = playerStat.Level;
        playerData.CurrentExp = playerStat.CurrentExp;
        playerData.MaxExp = playerStat.MaxExp;
        playerData.SkillPoint = playerStat.SkillPoint;
        playerData.Gold = playerStat.Gold;
        playerData.CurrentHp = playerStat.CurrentHp;
        playerData.MaxHp = playerStat.MaxHp;
        playerData.CurrentMp = playerStat.CurrentMp;
        playerData.MaxMp = playerStat.MaxMp;
        playerData.Atk = playerStat.Atk;
        playerData.Def = playerStat.Def;
        playerData.Critical = playerStat.Crit;
        playerData.CritDmg = playerStat.CritDmg;

        // 플레이어 위치 정보 저장
        Vector3 pos = PlayerStat.Instance.transform.position;
        playerData.PosX = pos.x;
        playerData.PosY = pos.y;
        playerData.PosZ = pos.z;

        // 인벤토리 아이템 저장
        playerData.InventoryItems.Clear();
        List<InventoryItem> allItems = InventoryManager.Instance.GetAllItems();

        foreach (var item in allItems)
        {
            ItemSaveData itemData = new ItemSaveData
            {
                id = item.ItemData.ID,
                quantity = item.Quantity,
                isEquipped = item.IsEquipped
            };
            playerData.InventoryItems.Add(itemData);
        }

        // 퀵슬롯 저장
        playerData.QuickSlots.Clear();
        for(int i = 0; i < quickSlotList.Count; i++)
        {
            var item = quickSlotList[i].GetItemData();
            if(item != null)
            {
                playerData.QuickSlots.Add(new QuickSlotSaveData
                {
                    slotIndex = i,
                    ItemId = item.ItemData.ID
                });
            }
        }

        // 스킬 저장
        playerData.LearnedSkills.Clear();
        foreach(var skill in skillBook.LearnedSkills)
        {
            playerData.LearnedSkills.Add(new SkillSaveData
            {
                id = skill.ID,
                level = skill.MaxLevel,
            });
        }
        

        // json 저장 
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePath, json);

        Debug.Log("게임 저장 완료" + filePath);
    }

    private void LoadGame()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("저장된 파일이 없습니다.");
            return;
        }

        string json = File.ReadAllText(filePath);
        PlayerSaveData playerData = JsonUtility.FromJson<PlayerSaveData>(json);

        // 플레이어 정보 및 스탯 로드
        playerStat.UserName = playerData.UserName;
        playerStat.Level = playerData.Level;
        playerStat.SkillPoint = playerData.SkillPoint;
        playerStat.Gold = playerData.Gold;
        playerStat.CurrentExp = playerData.CurrentExp;
        playerStat.CurrentHp = playerData.CurrentHp;
        playerStat.CurrentMp = playerData.CurrentMp;
        playerStat.Atk = playerData.Atk;
        playerStat.Def = playerData.Def;
        playerStat.Crit = playerData.Critical;
        playerStat.CritDmg = playerData.CritDmg;

        // 플레이어 위치 세이브 포인트
        Vector3 loadPos = new Vector3(playerData.PosX, playerData.PosY, playerData.PosZ);
        PlayerStat.Instance.transform.position = loadPos;

        // 인벤토리 아이템 로드
        Debug.Log("----------------인벤토리 로드 시작--------------");
        InventoryManager.Instance.ClearInventory(); // 기존 인벤토리 초기화 함수 필요
        foreach (var itemSave in playerData.InventoryItems)
        {
            // ID를 통해 ScriptableObject(ItemData)를 찾아와야 함
            ItemData data = GetItemDataByID(itemSave.id);
            if (data != null)
            {
                Debug.Log($"아이템 로드 성공: {data.name} (ID:{itemSave.id})");
                InventoryManager.Instance.LoadItem(data, itemSave.quantity, itemSave.isEquipped);
            }
            else
            {
                Debug.LogWarning($"아이템 데이터 못 찾음! ID = {itemSave.id}");
            }
        }

        // 인벤토리 퀵슬롯 초기화 후 로드
        Debug.Log("----------------퀵슬롯 로드 시작--------------");
        foreach(var slot in quickSlotList)
        {
            slot.ClearSlot(); // 퀵슬롯 초기화
        }

        foreach(var qsData in playerData.QuickSlots)
        {
            InventoryItem item = InventoryManager.Instance.GetAllItems().Find(i => i.ItemData.ID == qsData.ItemId);
            if (item != null && qsData.slotIndex < quickSlotList.Count)
            {
                Debug.Log($"퀵슬롯 {qsData.slotIndex}에 아이템 설정 시도: {item.ItemData.name}");
                quickSlotList[qsData.slotIndex].SetItemSlot(item);
            }
            else
            {
                Debug.LogWarning($"퀵슬롯 {qsData.slotIndex} 아이템 못 찾음 ID:{qsData.ItemId}");
            }
        }

        // 스킬 저장
        skillBook.ClearSkillBook();
        foreach (var skillSave in playerData.LearnedSkills)
        {
            SkillData skilldata = GetSkillDataByID(skillSave.id);
            if(skilldata != null)
            {
                Debug.Log($"스킬 데이터 로드 완료: {skilldata.name} (ID: {skillSave.id})");
                skillBook.LoadSkill(skilldata, skillSave.level);
            }
            else
            {
                // 여기가 null이라면 Resources 경로 혹은 ID 불일치 문제입니다.
                Debug.LogError($"SaveManager: ID {skillSave.id}에 해당하는 SkillData를 Resources/Skills에서 찾을 수 없습니다.");
            }
        }
        
        Debug.Log("게임 불러오기 완료");
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    private ItemData GetItemDataByID(int id)
    {
        // 방법 1: Resources.LoadAll<ItemData>("") 사용
        // 방법 2: 별도의 ItemDatabase를 만들어 관리
        ItemData[] allData = Resources.LoadAll<ItemData>("Items"); // Items 폴더 내의 모든 ItemData
        foreach (var data in allData)
        {
            if (data.ID == id) return data;
        }
        return null;
    }

    private SkillData GetSkillDataByID(int id)
    {
        SkillData[] allSkills = Resources.LoadAll<SkillData>("Skills"); // 경로 주의!
        foreach (var skill in allSkills)
        {
            if (skill.ID == id) return skill;
        }
        return null;
    }
}
