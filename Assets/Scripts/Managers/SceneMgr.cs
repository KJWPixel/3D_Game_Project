using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr Instance { get; private set; }
    public SCENE CurrentScene { get; private set; }
    public SCENE NextScene { get; private set; }

    [Header("씬 이름 매핑")]
    [SerializeField] private string[] sceneNames;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    
    public void ChangeScene(SCENE target, bool loading = false)
    {
        if (CurrentScene == target) return;//현재 씬과 같다면 리턴

        if (loading)
        {
            NextScene = target;
            SceneManager.LoadScene((int)SCENE.LOADING);
            return;
        }

        //즉시 로드
        CurrentScene = target;
        SceneManager.LoadScene((int)CurrentScene);
    }

    public void ChangeSceneByName(string sceneName, bool loading =false)
    {
        if(loading)
        {

        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
