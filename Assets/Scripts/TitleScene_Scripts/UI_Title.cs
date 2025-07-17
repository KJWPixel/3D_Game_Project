using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Title : MonoBehaviour
{
    void Start()
    {

    }


    void Update()
    {
       
    }

    public void TitleTestButton()
    {
        Debug.Log("--TitleTestButton--");
    }

    public void TitleContinueButton()
    {

    }

    public void TitleStartButton()
    {
        //SceneManager.LoadScene(1);//추후 SceneManager클래스로 관리 현재는 임시적
        Shared.SceneManager.ChangeScene(SCENE.LOBBY);
    }

    public void TitleLoadButton()
    {
        //플레이어 저장된 위치값과 데이터를 가져옴
    }

    public void TitleOptionButton()
    {
        //옵션을 여는 버튼
    }

    public void TitleExitButton()
    {
        //게임종료 버튼은 크리티컬하기 때문에 나중에 별도의 팝업으로 한번 더 확인하는 기능 만들기
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

    


