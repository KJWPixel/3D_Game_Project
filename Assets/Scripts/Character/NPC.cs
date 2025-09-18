using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : NPCCharacter
{
    private void Start()
    {
        if(Player == null)
        {
            Player = GameManager.Instance.Player;
        }  
    }
    private void Update()
    {
        InteractDialog();
    }

    private void InteractDialog()
    {
        float Distance = Vector3.Distance(transform.position, Player.transform.position);
        if(Distance < InsteractionRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.Instance.StartDialogue(Name, DialogueLines);
        }
    }
}
