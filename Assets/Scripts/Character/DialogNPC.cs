using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum InteractionType
{
    None,   
    Shop,
    Quest,
}

public class DialogNPC : NPCCharacter
{
    [Header("NPC 대화 옵션")]
    public InteractionType interactionType;
    public QuestData QuestData;
    public Transform TargetTrs;

    private Camera MainCamera;

    private void Start()
    {
        MainCamera = Camera.main;

        //if(NPCText != null )
        //{
        //    NPCText = NPCMark.gameObject.GetComponent<TextMeshPro>();
        //}
        if (NPCText == null && NPCMark != null)
        {
            NPCText = NPCMark.gameObject.GetComponent<TextMeshPro>();
        }

        if (Player == null)
        {
            Player = GameManager.Instance.Player;
        }


        NPCSetup();
    }
    private void Update()
    {
        Interact();
    }

    private void LateUpdate()
    {
        NPCNameOn();
        //NPCQuestOn();
    }

    private void NPCSetup()
    {
        if(NPCText != null)
        {
            NPCText.text = NpcName;
        }
        
        if(NPCMark != null)
        {
            NPCMark.gameObject.SetActive(false);
        }
        
    }

    private void NPCNameOn()
    {
        Playerdistance = Vector3.Distance(transform.position, Player.transform.position);
        if (Playerdistance > NameDistance || Player == null)
        {
            NPCMark.gameObject.SetActive(false);
            return;
        }
        else if(Playerdistance < NameDistance)
        {
            NPCMark.gameObject.SetActive(true);
            Quaternion targetRotation = MainCamera.transform.rotation;
            NPCMark.transform.rotation = targetRotation;
        }
    }

    private void NPCQuestOn()
    {
        Playerdistance = Vector3.Distance(transform.position, Player.transform.position);
        if (Playerdistance > NameDistance || Player == null)
        {
            NPCQuestMark.gameObject.SetActive(false);
            return;
        }
        else if (Playerdistance < NameDistance)
        {
            NPCQuestMark.gameObject.SetActive(true);
            Quaternion targetRotation = MainCamera.transform.rotation;
            NPCQuestMark.transform.rotation = targetRotation;
        }
    }

    private void Interact()
    {
        Playerdistance = Vector3.Distance(transform.position, Player.transform.position);

        if (isTolk && Playerdistance > InsteractionRange)
        {
            isTolk = false;
            DialogueManager.Instance.EndDialogue(); 
            return;
        }

        if (Playerdistance < InsteractionRange && Input.GetKeyDown(KeyCode.E))
        {
            isTolk = true;
            Debug.Log("StartDialogue");

            DialogueManager.Instance.StartDialogue(this, Name, DialogueLines, interactionType);
        }
    }
}
