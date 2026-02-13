using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [Header("FPS Settings")]
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private float updateInterval = 0.5f;

    private float accum = 0f;
    private int frames = 0;
    private float timeLeft;

    private void Start()
    {
        timeLeft = updateInterval;
    }

    private void Update()
    {
        timeLeft -= Time.unscaledDeltaTime; //프레임 영향을 안받는 시간 사용
        accum += Time.unscaledDeltaTime;
        frames++;

        if(timeLeft <= 0.0)
        {
            float fps = frames / accum;
            fpsText.text = string.Format("{0:F0} FPS", fps);

            fpsText.color = (fps >= 60 ) ? Color.green : (fps >= 30) ? Color.yellow : Color.red;

            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}
