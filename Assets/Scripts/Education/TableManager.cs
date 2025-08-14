using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TableManager
{
    public TableStage Stage = new TableStage();

    public void Init()
    {
#if UNITY_EDITOR
        Stage.Init_Csv("Stage", 2, 0);//2��° �� ���� 0�� ������ 
#else
        Stage.Init_Binary("Stage");
#endif
    }

    public void Save()
    {
        Stage.Save_Binary("Stage");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
