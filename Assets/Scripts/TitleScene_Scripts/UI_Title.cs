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
        //SceneManager.LoadScene(1);//���� SceneManagerŬ������ ���� ����� �ӽ���
        Shared.SceneManager.ChangeScene(SCENE.LOBBY);
    }

    public void TitleLoadButton()
    {
        //�÷��̾� ����� ��ġ���� �����͸� ������
    }

    public void TitleOptionButton()
    {
        //�ɼ��� ���� ��ư
    }

    public void TitleExitButton()
    {
        //�������� ��ư�� ũ��Ƽ���ϱ� ������ ���߿� ������ �˾����� �ѹ� �� Ȯ���ϴ� ��� �����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

    


