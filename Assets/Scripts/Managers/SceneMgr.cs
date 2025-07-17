using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public SCENE Scene;

    private void Awake()
    {
        if (Shared.SceneManager == null)
        {
            Shared.SceneManager = this;

            DontDestroyOnLoad(this);
        }
    }
    
    public void ChangeScene(SCENE _e, bool _Loading = false)
    {
        if (Scene == _e)
            return;

        Scene = _e;

        if (_Loading)
        {
            SceneManager.LoadScene((int)SCENE.LOADING);
            return;
        }

        switch (_e)//리셋이 필요할때 사용
        {
            case SCENE.TITLE:
                break;
            case SCENE.LOBBY:
                break;
            case SCENE.MAIN:
                break;
            case SCENE.ENG:
                break;
        }

        SceneManager.LoadScene((int)Scene);
    }
}
