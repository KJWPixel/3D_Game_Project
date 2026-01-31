using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;      // 기본 커서 이미지
    public Texture2D cursorPressTexture; // 클릭 시 이미지
    public Vector2 hotSpot = Vector2.zero; // 커서의 클릭 중심점 (보통 좌측 상단 0,0)

    void Start()
    {
        // 시작할 때 기본 커서로 설정
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void Update()
    {
        // 마우스를 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorPressTexture, hotSpot, CursorMode.Auto);
        }

        // 마우스를 뗐을 때 다시 기본으로
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        }
    }
}
