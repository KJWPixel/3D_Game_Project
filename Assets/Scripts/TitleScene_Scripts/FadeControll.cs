using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeControll : MonoBehaviour
{
    //TitleScene ���̵� �� �ƿ� ��Ʈ��
    [Header("���̵� �̹���")]
    [SerializeField] Image FadeImage;

    [Header("���̵� �ð�")]
    [SerializeField] float FadeTime;

    [Header("���̵� ��")]
    [SerializeField] bool FadeCheck;

    Color color;

    void Start()
    {
        
    }

    
    void Update()
    {
        FadeIn();
    }

    private void FadeIn()//��Ӵٰ� ���� �����
    {
        if(FadeCheck == true && FadeImage.color.a <= 1f)
        {                     
            color = FadeImage.color;
            color.a -= Time.deltaTime / FadeTime;

            if(color.a < 0f)
            {
                color.a = 0f;
            }

            FadeImage.color = color; 
        }
    }
    
    private void FadeOut()//��Ҵٰ� ���� ��ο���
    {
        if(FadeCheck == true && FadeImage.color.a >= 1f)
        {
            color = FadeImage.color;
            color.a += Time.deltaTime / FadeTime;

            if(color.a > 1f)
            {
                color.a = 1f;
            }
            FadeImage.color = color;
        }
    }
}
