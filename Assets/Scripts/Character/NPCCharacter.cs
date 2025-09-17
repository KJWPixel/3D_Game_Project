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
    [SerializeField] protected float InsteractionRang = 0f;
    [SerializeField] protected string[] DialogLine;
    [SerializeField] protected int CurrentDialogIndex = 0;
    [SerializeField] protected Image TextPanel;
    [SerializeField] protected TextMeshProUGUI Text;
    void Start()
    {
        
    }

    
    void Update()
    {
       
    }
}
