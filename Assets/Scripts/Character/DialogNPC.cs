using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    None,
    Choice,
    Shop,
    Quest,
}

public class DialogNPC : NPCCharacter
{
    public InteractionType interactionType;
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
        DialogueManager.Instance.CloseDialogue(isTolk);

        if (isTolk && distance > InsteractionRange)
        {
            isTolk = false;
            DialogueManager.Instance.EndDialogue(); 
            return;
        }

        if (distance < InsteractionRange && Input.GetKeyDown(KeyCode.E))
        {
            isTolk = true;
            DialogueManager.Instance.StartDialogue(Name, DialogueLines, interactionType);
        }
    }
}
