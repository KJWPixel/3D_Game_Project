[System.Serializable]
public class GameSettings
{
    //�׷��ȿɼ�
    public int Screen;
    public int Resolution;
    public int FrameRate;
    //����
    public float MasterVolume;
    public float EffectVolume;
    public float BackGroundVolume;
    //�����÷���
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
