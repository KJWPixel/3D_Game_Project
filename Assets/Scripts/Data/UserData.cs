using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public string username;
    public string password;//(권장: 해시로 변경하지만 지금은 기존 호환용)
    //게임 진행에 따라 변경되는 필드 예시
    public int level;
    public int golds;
    public string lastSavedAt;
        
}