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
    [SerializeField] protected string name; 
    [TextArea] [SerializeField] protected string[] DialogueLines;
    [SerializeField] protected int CurrentDialogueIndex = 0;
    [SerializeField] protected float InsteractionRange = 0f;
    [SerializeField] protected float distance = 0f;
    [SerializeField] protected bool isTolk = false;

    public int Id => id;
    public string Name => name;
    public bool IsTolk => isTolk;

    public void asjnd()
    {

    }
}
