using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private string TableName = "NPC Table";

    [SerializeField] private List<string> Sentences = new List<string>();
    [SerializeField] public int Index = 0;
    [SerializeField] private Coroutine TypingCoroutine;
    [SerializeField] private bool IsTyping;
    [SerializeField] private InteractionType CurrentInteraction = InteractionType.None;
    [SerializeField] private float typingDelay = 0.03f;
    public DialogNPC CurrentNPC;

    // 대화 중인지 외부에서 확인
    public bool IsDialogueActive => UIManager.Instance.DialoguePanel.activeInHierarchy;
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

        Index = 0; // 반도시 인덱스 초기화

        // UI 세팅
        UIManager.Instance.DialoguePanel.SetActive(true);
        UIManager.Instance.NameText.text = _Name;

        // 첫 문장 표시
        ShowTextSentence();
    }

    // ID 범위를 이용해 대화를 시작하는 함수 추가
    public void StartDialogueWithLocalization(DialogNPC _NPC, InteractionType type)
    {
        CurrentNPC = _NPC;
        CurrentInteraction = type;
        Sentences.Clear();

        // 1. NPC 이름 번역
        string translatedName = LocalizationSettings.StringDatabase.GetLocalizedString(TableName, _NPC.npcNameKey);
        UIManager.Instance.NameText.text = translatedName;

        // 2. 대사 리스트 번역 및 할당
        foreach (string key in _NPC.dialogueKeys)
        {
            string translatedLine = LocalizationSettings.StringDatabase.GetLocalizedString(TableName, key);
            Sentences.Add(translatedLine);
        }

        // 3. 퀘스트 문구 (시스템 테이블이나 공통 키 활용)
        if(_NPC.QuestData != null && type == InteractionType.Quest)
        {
            string questAsk = LocalizationSettings.StringDatabase.GetLocalizedString(TableName, "NPC_Quest_Accept");
            Sentences.Add(questAsk);
        }

        Index = 0;
        UIManager.Instance.DialoguePanel.SetActive(true);
        ShowTextSentence();

    }

    private void ShowTextSentence()
    {
        // 모든 문장 끝 -> 대화종료
        if (Index >= Sentences.Count)
        {
            EndDialogue();
            return;
        }

        // 타이핑 중인데 플레이엉가 입력하면 즉시 전체 표시
        if(IsTyping)
        {
            if(TypingCoroutine != null)
            {
                StopCoroutine(TypingCoroutine);
                TypingCoroutine = null;
            }
            UIManager.Instance.DialogueText.text = Sentences[Index];
            IsTyping = false;
            return;
        }

        TypingCoroutine = StartCoroutine(TextCoroutine());

        //if (Index < Sentences.Count) // Lenght -> Count로 변경 (List)
        //{
        //    if (TypingCoroutine != null)
        //    {
        //        StopCoroutine(TypingCoroutine);
        //        TypingCoroutine = null;
        //    }    

        //    TypingCoroutine = StartCoroutine(TextCoroutine());
        //    Index++;
        //}
        //else
        //{
        //    EndDialogue();
        //}
    }

    IEnumerator TextCoroutine() // 타이핑 코루틴 
    {
        IsTyping = true;
        UIManager.Instance.DialogueText.text = "";

        // Index는 아지가 증가 전이어야 하므로, 여기서 현재 문장 사용
        string currnetSentence = Sentences[Index];

        foreach (char c in Sentences[Index])
        {
            UIManager.Instance.DialogueText.text += c;
            yield return new WaitForSeconds(typingDelay);
        }

        // 더 이상 문장이 없으면 종류
        //if (Index >= Sentences.Count)
        //{
        //    EndDialogue();
        //}

        IsTyping = false;
        TypingCoroutine = null;
    }

    public void ContinueDialogue()
    {
        if (!IsDialogueActive) return;

        if (IsTyping)
        {
            // 타이핑 중 → 즉시 완료
            if (TypingCoroutine != null)
            {
                StopCoroutine(TypingCoroutine);
                TypingCoroutine = null;
            }
            UIManager.Instance.DialogueText.text = Sentences[Index];
            IsTyping = false;
        }
        else
        {
            // 타이핑 끝 → 다음 문장
            Index++;
            ShowTextSentence();  // 문장 다 끝나면 여기서 EndDialogue() 자동 호출됨
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
                ShopManager.Instance.OpenShop(CurrentNPC);
                UIManager.Instance.DialoguePanel.SetActive(false);
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
        IsTyping = false;
        TypingCoroutine = null;
        CurrentInteraction = InteractionType.None;
    }

    public void CloseDialogue(bool _IsTolk)
    {
        if(!_IsTolk)
        {
            UIManager.Instance.DialoguePanel.SetActive(false);
            Index = 0;
            IsTyping = false;
            if (TypingCoroutine != null)
            {
                StopCoroutine(TypingCoroutine);
                TypingCoroutine = null;
            }

        }
    }
}
