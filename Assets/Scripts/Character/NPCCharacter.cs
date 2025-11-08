using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class NPCCharacter : MonoBehaviour
{
    protected GameObject Player;

    [Header("기본정보")]
    [SerializeField] protected int id;
    [SerializeField] protected string NPCname;
    [SerializeField] protected TextMeshPro NPCText;
    [SerializeField] protected GameObject NPCMark;
    [SerializeField] protected int NameDistance = 10;

    [Header("대화 DialogueLine")]
    [TextArea] [SerializeField] protected string[] DialogueLines;
    [SerializeField] protected int CurrentDialogueIndex = 0;
    [SerializeField] protected float InsteractionRange = 3f;
    [SerializeField] protected float Playerdistance;
    [SerializeField] protected bool isTolk = false;

    public int Id => id;
    public string Name => NPCname;
    public bool IsTolk => isTolk;
}
