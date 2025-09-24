using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] public PlayerStat PlayerStat;

    PlayerSaveData playerSaveData;
    string filePath;

    [System.Serializable]
    public class PlayerSaveData
    {
        public string UserName;
        public int Level;
        public float CurrentExp;
        public int SkillPoint;

        public float CurrentHp;
        public float CurrentMp;

        public float Atk;
        public float Def;
        public float Critical;
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
        PlayerSaveData playerSaveData = new PlayerSaveData();

        playerSaveData.UserName = PlayerStat.UserName;
        playerSaveData.Level = PlayerStat.Level;
        playerSaveData.CurrentExp = PlayerStat.CurrentExp;
        playerSaveData.SkillPoint = PlayerStat.SkillPoint;
        playerSaveData.CurrentHp = PlayerStat.CurrentHp;
        playerSaveData.CurrentMp = PlayerStat.CurrentMp;
        playerSaveData.Atk = PlayerStat.Atk;
        playerSaveData.Def = PlayerStat.Def;
        playerSaveData.Critical = PlayerStat.Crit;

        string json = JsonUtility.ToJson(playerSaveData, true);
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
        PlayerSaveData playerSaveData = JsonUtility.FromJson<PlayerSaveData>(json);

        // 저장된 데이터 → PlayerStats에 적용
        PlayerStat.UserName = playerSaveData.UserName;
        PlayerStat.Level = playerSaveData.Level;
        PlayerStat.SkillPoint = playerSaveData.SkillPoint;
        PlayerStat.CurrentExp = playerSaveData.CurrentExp;
        PlayerStat.CurrentHp = playerSaveData.CurrentHp;
        PlayerStat.CurrentMp = playerSaveData.CurrentMp;
        PlayerStat.Atk = playerSaveData.Atk;
        PlayerStat.Def = playerSaveData.Def;
        PlayerStat.Crit = playerSaveData.Critical;

        Debug.Log("게임 불러오기 완료");
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}
