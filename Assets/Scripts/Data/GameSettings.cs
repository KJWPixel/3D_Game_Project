[System.Serializable]
public class GameSettings
{
    //그래픽옵션
    public int Screen;
    public int Resolution;
    public int FrameRate;
    //사운드
    public float MasterVolume;
    public float EffectVolume;
    public float BackGroundVolume;
    //게임플레이
    public int language;

    public GameSettings()
    {
        Screen = 0;//Index
        Resolution = 0;//Index
        FrameRate = 60;
        MasterVolume = 1f;
        EffectVolume = 1f;
        BackGroundVolume = 1f;
        language = 0;//Index
    }
}
