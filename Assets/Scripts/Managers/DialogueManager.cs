using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private string[] Sentences;
    private int Index;
    private Coroutine TypingCoroutine;
    private bool IsTyping;

    private void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(string _Name, string[] _DialogueLine)
    {
        Sentences = _DialogueLine;
        Index = 0;

        UI_Manager.Instance.DialoguePanel.SetActive(true);
        UI_Manager.Instance.NameText.text = _Name;
    }

    private void ShowTextSentences()
    {
        if(Index < Sentences.Length)
        {

        }
    }
}
