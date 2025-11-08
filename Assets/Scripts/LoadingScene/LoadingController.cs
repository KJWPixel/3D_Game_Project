using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private LoadingUI loadingUI;

    private void Start()
    {
        StartCoroutine(DoLoad());
    }

    private IEnumerator DoLoad()
    {
        //안전하게 SceneMgr.Instance가 준비될 떄 까지 대기
        float wait = 0f;
        while (SceneMgr.Instance == null)
        {
            yield return null;
            wait += Time.unscaledDeltaTime;
            if(wait > 5f) break; 
        }

        SCENE target = SceneMgr.Instance != null ? SceneMgr.Instance.NextScene : SCENE.MAIN;
        AsyncOperation op = SceneManager.LoadSceneAsync((int)target);
        op.allowSceneActivation = false;
        
        float displayed = 0f;
        while(!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            displayed = Mathf.MoveTowards(displayed, progress, Time.unscaledDeltaTime * 0.6f);
            loadingUI?.SetProgress(displayed);

            if(progress >= 1f)
            {
                loadingUI?.ShowPressAnyKey(true);
                if(Input.anyKeyDown)
                {
                    op.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
