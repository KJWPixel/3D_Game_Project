using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicTestManager : MonoBehaviour
{
    [SerializeField] Button Button;
    [SerializeField] QuestData QuestData;

    public void AddQuestTest()
    {
        QuestManager.Instance.AddQuest(QuestData);
    }
}
