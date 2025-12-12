using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private List<string> Sentences = new List<string>();
    [SerializeField] public int Index = 0;
    [SerializeField] private Coroutine TypingCoroutine;
    [SerializeField] private bool IsTyping;
    [SerializeField] private InteractionType CurrentInteraction = InteractionType.None;
    [SerializeField] private float typingDelay = 0.03f;
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

    //기존 string[] 
    public void StartDialogue(string _Name, string[] _DialogueLine)
    {
        StartDialogue(null, _Name, (IEnumerable<string>)_DialogueLine, InteractionType.None);
    }

    public void StartDialogue(DialogNPC _NPC, string _Name, string[] _DialogueLine, InteractionType _Type)
    {
        StartDialogue(_NPC, _Name, (IEnumerable<string>)_DialogueLine, _Type);
    }

    public void StartDialogue(DialogNPC _NPC, string _Name, IEnumerable<string> _DialogueLines, InteractionType _Type)
    {
        CurrentNPC = _NPC;
        CurrentInteraction = _Type;

        // 안전하게 내부 리스트로 복사 (원본 변경 금지)
        Sentences.Clear();

        // 전달받은 대사들 안전하게 복사 (원본 List 보호)
        if (_DialogueLines != null)
        {
            Sentences.AddRange(_DialogueLines);
        }
            
        // 퀘스트 NPC이고, 퀘스트 타입이면 마지막 
        if (_NPC != null && _NPC.QuestData != null && _Type == InteractionType.Quest)
        {
            Sentences.Add("퀘스트가 있습니다. 수락하시겠습니까?");
        }

        // 인덱스 초기화
        //Index = 0;

        // UI 세팅
        UIManager.Instance.DialoguePanel.SetActive(true);
        UIManager.Instance.NameText.text = _Name;

        // 첫 문장 표시
        ShowTextSentence();
    }

    private void ShowTextSentence()
    {
        if (Index < Sentences.Count) // Lenght -> Count로 변경 (List)
        {
            if (TypingCoroutine != null)
            {
                StopCoroutine(TypingCoroutine);
                TypingCoroutine = null;
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

        // Index는 아지가 증가 전이어야 하므로, 여기서 현재 문장 사용
        string currnetSentence = Sentences[Index];

        foreach (char s in Sentences[Index])
        {
            UIManager.Instance.DialogueText.text += s;
            yield return new WaitForSeconds(typingDelay);
        }


        // 더 이상 문장이 없으면 종류
        if (Index >= Sentences.Count)
        {
            EndDialogue();
        }

        IsTyping = false;
    }

    public void EndDialogue()
    {
        switch (CurrentInteraction)
        {
            case InteractionType.None:
                UIManager.Instance.DialoguePanel.SetActive(false);
                break;
            case InteractionType.Shop:
                ShopManager.Instance.OpenShop(CurrentNPC);
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
