using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioMixer audioMixer;

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
    }

    private void Start()
    {
        //�ʱ� ���� ���� ����
        ApplySoundSettings(SettingsManager.Instance.GetSettings());
    }

    public void ApplySoundSettings(GameSettings settings)
    {
        //AudioMixer�� ���� ����
        audioMixer.SetFloat("MasterVolume", LinearToDecibel(settings.MasterVolume));
        audioMixer.SetFloat("EffectVolume", LinearToDecibel(settings.EffectVolume));
        audioMixer.SetFloat("BackGroundVolume", LinearToDecibel(settings.BackGroundVolume));
        Debug.Log($"���� ���� ����: Master = {settings.MasterVolume}, Effect = {settings.EffectVolume}, BGM = {settings.BackGroundVolume}");      
    }

    private float LinearToDecibel(float linear)
    {
        if (linear <= 0) return -80f;
        return Mathf.Log10(linear) * 20f;
    }
}
