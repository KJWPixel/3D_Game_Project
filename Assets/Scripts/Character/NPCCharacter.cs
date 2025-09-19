using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class NPCCharacter : MonoBehaviour
{
    protected GameObject Player;

    [Header("기본정보")]
    [SerializeField] protected string Name;    
    [TextArea] [SerializeField] protected string[] DialogueLines;
    [SerializeField] protected int CurrentDialogueIndex = 0;
    [SerializeField] protected float InsteractionRange = 0f;
}
