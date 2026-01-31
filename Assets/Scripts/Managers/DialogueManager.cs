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

    private QuestData currentTalkQuest;

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
        if (_NPC != null && _NPC.questList != null && _Type == InteractionType.Quest)
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
        currentTalkQuest = null;

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
        if (type == InteractionType.Quest)
        {
            currentTalkQuest = _NPC.GetAvailableQuest(); // NPC로부터 다음 퀘스트를 가져옴

            if (currentTalkQuest != null)
            {
                // 선행 퀘스트 조건 체크
                if (currentTalkQuest.PrerequisiteQuest != null &&
                    !QuestManager.Instance.ClearQuests.Contains(currentTalkQuest.PrerequisiteQuest.QuestId))
                {
                    // 선행 퀘스트를 안 깼을 때: 문구 추가 및 인터랙션 타입 변경 (버튼 안 나오게)
                    string prereqMsg = LocalizationSettings.StringDatabase.GetLocalizedString(TableName, "NPC_Quest_Prerequisite_NotMet");
                    Sentences.Add(prereqMsg);
                    CurrentInteraction = InteractionType.None; // 중요: EndDialogue에서 버튼이 안 뜨게 함
                }
                else
                {
                    // 선행 퀘스트를 깼을 때: 수락 문구 추가
                    string questAsk = LocalizationSettings.StringDatabase.GetLocalizedString(TableName, "NPC_Quest_Accept");
                    Sentences.Add(questAsk);
                }
            }
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
                if (currentTalkQuest != null)
                {
                    // ❗ 중요: NPC.QuestData가 아니라 여기서 확정된 currentTalkQuest를 전달합니다.
                    UIManager.Instance.SetupQuestButton(currentTalkQuest);
                    UIManager.Instance.ChoiceYes.SetActive(true);
                    UIManager.Instance.ChoiceNo.SetActive(true);
                }
                else
                {
                    UIManager.Instance.DialoguePanel.SetActive(false);
                }
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
