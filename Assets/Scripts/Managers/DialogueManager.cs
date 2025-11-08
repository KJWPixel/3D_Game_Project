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

    public DialogNPC CurrentNPC;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(string _Name, string[] _DialogueLine)
    {
        Sentences = _DialogueLine;

        UIManager.Instance.DialoguePanel.SetActive(true);
        UIManager.Instance.NameText.text = _Name;

        ShowTextSentences();
    }

    public void StartDialogue(DialogNPC _NPC, string _Name, string[] _DialogueLine, InteractionType _Type)
    {
        CurrentNPC = _NPC;
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

    public void EndDialogue()
    {
        switch (CurrentInteraction)
        {
            case InteractionType.None:
                UIManager.Instance.DialoguePanel.SetActive(false);
                break;
            case InteractionType.Shop:
                UIManager.Instance.ChoiceYes.SetActive(true);
                UIManager.Instance.ChoiceNo.SetActive(true);
                UIManager.Instance.SetupQuestButton(CurrentNPC.QuestData);
                break;
            case InteractionType.Quest:
                UIManager.Instance.ChoiceYes.SetActive(true);
                UIManager.Instance.ChoiceNo.SetActive(true);
                UIManager.Instance.SetupQuestButton(CurrentNPC.QuestData);
                // QuestData를 버튼 클릭 이벤트에 전달             
                break;
            default:
                UIManager.Instance.DialoguePanel.SetActive(false);
                break;
        }
    
        Index = 0;       
    }

    public void CloseDialogue(bool _IsTolk)
    {
        if(!_IsTolk)
        {
            UIManager.Instance.DialoguePanel.SetActive(false);
        }
    }
}
