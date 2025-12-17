using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject BgmPlayer; //BGM 
    [SerializeField] private GameObject SfxPlayer; //SFX

    [Header("BGM")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip titleBGMClip;
    [SerializeField] private AudioClip loadingBGMClip;
    [SerializeField] private AudioClip inGameBGMClip;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("SFX Clip")]
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private AudioClip runningClip;
    [SerializeField] private AudioClip blowClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip levelUpClip;
    [SerializeField] private AudioClip deadClip;
    [SerializeField] private AudioClip inventoryOpenClip;
    [SerializeField] private AudioClip inventoryCloseClip;
    [SerializeField] private AudioClip shopBuyClip;
    [SerializeField] private AudioClip questOpenClip;
    [SerializeField] private AudioClip questCloseClip;
    [SerializeField] private AudioClip optionOpenClip;
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip skillSlashClip;
    [SerializeField] private AudioClip skillChargeSlashClip;
    [SerializeField] private AudioClip skillIceClip;


    // clip 추가

    private Dictionary<SFXType, AudioClip> sfxClips;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void SfxClipInit()
    {
        sfxClips = new Dictionary<SFXType, AudioClip>
        {
            { SFXType.Walking, walkingClip},
            { SFXType.Running, runningClip},
            { SFXType.Blow, blowClip},
            { SFXType.Hit, hitClip},
            { SFXType.LevelUp, levelUpClip},
            { SFXType.Dead, deadClip},
            { SFXType.InventoryOpen, inventoryOpenClip},
            { SFXType.InventoryClose, inventoryCloseClip},
            { SFXType.ShopBuy, shopBuyClip},
            { SFXType.QuestOpen, questOpenClip},
            { SFXType.QuestClose, questCloseClip},
            { SFXType.OptionOpen, optionOpenClip},
            { SFXType.ButtonClick, buttonClickClip},
            { SFXType.SkillSlash, skillSlashClip},
            { SFXType.SkillChargeSlash, skillChargeSlashClip},
            { SFXType.SkillIce, skillIceClip},
            //추가
        };
    }

    private void Start()
    {
        ApplySoundSettings(SettingsManager.Instance.GetSettings()); // 시작 사운드 적용

        //시작 씬에서 타이틀 BGM 재생
        if (SceneManager.GetActiveScene().name == "Title_Scene")
        {
            PlayBGM(titleBGMClip);
        }
    }

    //BGM 관리
    private void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmAudioSource == null || clip == null) return;

        if (bgmAudioSource.isPlaying && bgmAudioSource.clip == clip) return; // 이미 재생중이면 무시 

        bgmAudioSource.clip = titleBGMClip;

        bgmAudioSource.Stop();
        bgmAudioSource.clip = clip;
        bgmAudioSource.loop = loop;
        bgmAudioSource.Play();
        Debug.Log($"{clip.name}BGM 재생 시작");
    }

    public void StopBGM()
    {
        if (bgmAudioSource != null )
        {
            bgmAudioSource.Stop();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Title_Scene":
                PlayBGM(titleBGMClip);
                break;
            case "Loading_Scene":
                PlayBGM(loadingBGMClip);
                break;
            case "Main_Scene":
                PlayBGM(inGameBGMClip);
                break;
            //추가 씬이 존재한다면 추가
        }
    }

    public void PlaySFX(SFXType type)
    {
        if(!sfxClips.TryGetValue(type, out AudioClip clip) || clip == null)
        {
            Debug.LogWarning($"SFX 클립 없음 {clip}");
            return;
        }

        //Unity Voice Limit(32개) 
        sfxAudioSource.PlayOneShot(clip);
    }

    // ===볼륨 적용===
    public void ApplySoundSettings(GameSettings settings)
    {
        //AudioMixer에 볼륨 적용
        audioMixer.SetFloat("MasterVolume", LinearToDecibel(settings.MasterVolume));
        audioMixer.SetFloat("EffectVolume", LinearToDecibel(settings.EffectVolume));
        audioMixer.SetFloat("BackGroundVolume", LinearToDecibel(settings.BackGroundVolume));
        Debug.Log($"사운드 설정 적용: Master = {settings.MasterVolume}, Effect = {settings.EffectVolume}, BGM = {settings.BackGroundVolume}");      
    }

    private float LinearToDecibel(float linear)
    {
        if (linear <= 0) return -80f;
        return Mathf.Log10(linear) * 20f;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
