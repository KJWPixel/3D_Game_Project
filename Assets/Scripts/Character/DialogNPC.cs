using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Shop,
    Quest,
}

public class DialogNPC : NPCCharacter
{
    public InteractionType interactionType;
    public QuestData QuestData;
    public Transform TargetTrs;

    private void Start()
    {
        if (Player == null)
        {
            Player = GameManager.Instance.Player;
        }
    }
    private void Update()
    {
        Interact();

    }

    private void Interact()
    {
        distance = Vector3.Distance(transform.position, Player.transform.position);

        if (isTolk && distance > InsteractionRange)
        {
            isTolk = false;
            DialogueManager.Instance.EndDialogue(); 
            return;
        }

        if (distance < InsteractionRange && Input.GetKeyDown(KeyCode.E))
        {
            isTolk = true;
            Debug.Log("StartDialogue");
            DialogueManager.Instance.StartDialogue(this, Name, DialogueLines, interactionType);

            if (QuestManager.Instance.ClearQuests.Contains(QuestData.QuestId))
            {
                Debug.Log("이미 완료한 퀘스트입니다.");
                return;
            }

            QuestManager.Instance.UpdateQuestPrecess(QuestData.QuestClassification, id, QuestData.Amount);
        }
    }
}
