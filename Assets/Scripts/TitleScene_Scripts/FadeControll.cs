using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeControll : MonoBehaviour
{
    //TitleScene 페이드 인 아웃 컨트롤
    [Header("페이드 이미지")]
    [SerializeField] Image FadeImage;

    [Header("페이드 시간")]
    [SerializeField] float FadeTime;

    [Header("페이드 온")]
    [SerializeField] bool FadeCheck;

    Color color;

    void Start()
    {
        
    }

    
    void Update()
    {
        FadeIn();
    }

    private void FadeIn()//어둡다가 점점 밝아짐
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
    
    private void FadeOut()//밝았다가 점점 어두워짐
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
