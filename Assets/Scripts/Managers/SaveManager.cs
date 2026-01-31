using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] public PlayerStat PlayerStat;

    [SerializeField] private Transform CurrentTrsSave;

    PlayerSaveData playerSaveData;
    string filePath;

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
    }

    public class ItemSaveData
    {
        public int itemID;
        public int Quantity;
        public bool IsEquipped;
    }

    public class QuickSlotSaveData
    {
        public int index;
    }



    private void Awake()
    {
        filePath = Path.Combine(Application.dataPath, "playerData.json");        
    }

    private void Start()
    {
        LoadGame();
    }


    private void SaveGame()
    {
        PlayerSaveData playerData = new PlayerSaveData();

        playerData.UserName = PlayerStat.UserName;
        playerData.Level = PlayerStat.Level;
        playerData.CurrentExp = PlayerStat.CurrentExp;
        playerData.MaxExp = PlayerStat.MaxExp;
        playerData.SkillPoint = PlayerStat.SkillPoint;
        playerData.Gold = PlayerStat.Gold;
        playerData.CurrentHp = PlayerStat.CurrentHp;
        playerData.MaxHp = PlayerStat.MaxHp;
        playerData.CurrentMp = PlayerStat.CurrentMp;
        playerData.MaxMp = PlayerStat.MaxMp;
        playerData.Atk = PlayerStat.Atk;
        playerData.Def = PlayerStat.Def;
        playerData.Critical = PlayerStat.Crit;
        playerData.CritDmg = PlayerStat.CritDmg;

        Vector3 pos = PlayerStat.Instance.transform.position;
        playerData.PosX = pos.x;
        playerData.PosY = pos.y;
        playerData.PosZ = pos.z;

        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("게임 저장 완료");
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

        // 저장된 데이터 → PlayerStats에 적용
        PlayerStat.UserName = playerData.UserName;
        PlayerStat.Level = playerData.Level;
        PlayerStat.SkillPoint = playerData.SkillPoint;
        PlayerStat.Gold = playerData.Gold;
        PlayerStat.CurrentExp = playerData.CurrentExp;
        PlayerStat.CurrentHp = playerData.CurrentHp;
        PlayerStat.CurrentMp = playerData.CurrentMp;
        PlayerStat.Atk = playerData.Atk;
        PlayerStat.Def = playerData.Def;
        PlayerStat.Crit = playerData.Critical;

        Vector3 loadPos = new Vector3(playerData.PosX, playerData.PosY, playerData.PosZ);
        //PlayerStat.Instance.transform.position = loadPos;

        PlayerStat.Instance.transform.position = CurrentTrsSave.transform.position;

        Debug.Log("게임 불러오기 완료");
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}
