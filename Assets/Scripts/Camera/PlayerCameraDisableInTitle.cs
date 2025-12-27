using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCameraDisableInTitle : MonoBehaviour
{
    [Header("타이틀 씬")]
    [SerializeField] private string titleSceneName = "Title_Scene";

    private Camera playerCamera;
    private AudioListener playerAudioListener;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        playerAudioListener = GetComponentInChildren<AudioListener>();
    }

    private void Start()
    {
        
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("로드된 씬: " + scene.name);

        if (scene.name == titleSceneName)
        {
            if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
            if (playerAudioListener == null) playerAudioListener = GetComponentInChildren<AudioListener>();

            if (playerCamera != null) playerCamera.gameObject.SetActive(false);
            if (playerAudioListener != null) playerAudioListener.enabled = false;

            Debug.Log("타이틀 씬: 플레이어 카메라/AudioListener 비활성화");
        }
        else
        {
            if (playerCamera != null) playerCamera.gameObject.SetActive(true);
            if (playerAudioListener != null) playerAudioListener.enabled = true;

            Debug.Log("게임 씬: 플레이어 카메라/AudioListener 활성화");
        }
    }
}
