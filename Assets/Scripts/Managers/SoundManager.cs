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
    [SerializeField] private AudioClip skillSlashBlowClip;
    [SerializeField] private AudioClip skillChargeSlashClip;
    [SerializeField] private AudioClip skillSpinningCutClip;
    [SerializeField] private AudioClip skillIcePickClip;
    [SerializeField] private AudioClip skillEarthquakeClip;
    [SerializeField] private AudioClip skillFrostBladeClip;
    [SerializeField] private AudioClip skillColdWaveClip;
    [SerializeField] private AudioClip skillIceShowerClip;
    [SerializeField] private AudioClip skillMeteorFallClip;
    [SerializeField] private AudioClip skillEnergyCondensationClip;
    [SerializeField] private AudioClip skillHealingEnergyClip;
    [SerializeField] private AudioClip skillHealingBeaconClip;
    [SerializeField] private AudioClip skillLighthouseofMagicClip;
    [SerializeField] private AudioClip skillSpellofSwiftnessClip;
    [SerializeField] private AudioClip skillMagicShieldClip;
    [SerializeField] private AudioClip skillTeleportClip;
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
        SfxClipInit();
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
            { SFXType.SkillSlashBlow, skillSlashBlowClip},
            { SFXType.SkillChargeSlash, skillChargeSlashClip},
            { SFXType.SkillSpinningCut, skillSpinningCutClip },
            { SFXType.SkillIcePick, skillIcePickClip },
            { SFXType.SkillEarthQuake, skillEarthquakeClip },
            { SFXType.SkillFrostBlade, skillFrostBladeClip },
            { SFXType.SkillColdWave, skillColdWaveClip },
            { SFXType.SkillIceShower, skillIceShowerClip },
            { SFXType.SkillMeteorFall, skillMeteorFallClip },
            { SFXType.SkillEnergyCondensation, skillEnergyCondensationClip },
            { SFXType.SkillHealingEnergy, skillHealingEnergyClip },
            { SFXType.SkillHealingBeacon, skillHealingBeaconClip },
            { SFXType.SkillLighthouseofMagic, skillLighthouseofMagicClip },
            { SFXType.SkillSpellofSwiftness, skillSpellofSwiftnessClip },
            { SFXType.SkillMagicShield, skillMagicShieldClip },
            { SFXType.SkillTeleport, skillTeleportClip },
            
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
        if(type == SFXType.Walking || type == SFXType.Running)
        {
            if (sfxAudioSource.isPlaying && sfxAudioSource.clip == clip && sfxAudioSource) return; // 이미 재생 중이면 리턴

            sfxAudioSource.Stop();
            sfxAudioSource.clip = clip;
            sfxAudioSource.loop = true;
            sfxAudioSource.Play();
        }
        else
        {
            sfxAudioSource.PlayOneShot(clip);
        }
    }

    public void StopLoopSFX()
    {
        if (sfxAudioSource != null && sfxAudioSource.loop) //루프 중인 소리만 정지
        {
            sfxAudioSource.Stop();           // 재생 중인 루프 소리 정지
            sfxAudioSource.clip = null;      // 다음 재생 시 깨끗하게 시작하기 위해 클립 비우기 (선택적, 추천)
            sfxAudioSource.loop = false;           // sfxLoopSource.loop = false;  // 필요 시 루프 끄기 (이미 Stop() 하면 자동 처리됨)
            Debug.Log("루프 발소리 정지");
        }
    }

    public void PlayOneShot(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, position, volume);
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
