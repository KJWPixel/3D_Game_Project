using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private string[] Sentences;
    [SerializeField] public int Index;
    [SerializeField] private Coroutine TypingCoroutine;
    [SerializeField] private bool IsTyping;
    [SerializeField] private Enum CurrentInteraction;

    private void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(string _Name, string[] _DialogueLine)
    {
        Sentences = _DialogueLine;

        UIManager.Instance.DialoguePanel.SetActive(true);
        UIManager.Instance.NameText.text = _Name;

        ShowTextSentences();
    }

    public void StartDialogue(string _Name, string[] _DialogueLine, InteractionType _Type)
    {
        Sentences = _DialogueLine;
        CurrentInteraction = _Type;

        UIManager.Instance.DialoguePanel.SetActive(true);
        UIManager.Instance.NameText.text = _Name;

        ShowTextSentences();
    }

    private void ShowTextSentences()
    {
        if (Index < Sentences.Length)
        {
            if (TypingCoroutine != null)
            {
                StopCoroutine(TypingCoroutine);
            }
            
            TypingCoroutine = StartCoroutine(TextCoroutine());
            Index++;
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TextCoroutine()
    {
        IsTyping = true;
        UIManager.Instance.DialogueText.text = "";

        foreach (char s in Sentences[Index])
        {
            UIManager.Instance.DialogueText.text += s;

            yield return new WaitForSeconds(0.1f);
        }

        if (Index > Sentences.Length)
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        switch (CurrentInteraction)
        {
            case InteractionType.Choice:
                UIManager.Instance.ChoiceYes.SetActive(true);
                UIManager.Instance.ChoiceNo.SetActive(true);
                break;
            case InteractionType.Shop:
                UIManager.Instance.OnClickShop();
                break;
            case InteractionType.Quest:
                // Quest 관련 버튼/행동 활성화
                break;
            default:
                UIManager.Instance.DialoguePanel.SetActive(false);
                break;
        }

        Index = 0;
    }
}
