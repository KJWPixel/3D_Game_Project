using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("UI Key ¼³Á¤")]
    public KeyCode Option = KeyCode.Escape;
    public KeyCode Inventory = KeyCode.I;
    public KeyCode Skill = KeyCode.K;
    public KeyCode Quest = KeyCode.J;

    public event Action OnToggleOption;
    public event Action OnToggleInventory;
    public event Action OnToggleSkill;  
    public event Action OnToggleQuest;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            GameObject root = gameObject.transform.root.gameObject;
            DontDestroyOnLoad(root);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(Option)) { OnToggleOption?.Invoke(); }
        if (Input.GetKeyDown(Inventory)) { OnToggleInventory?.Invoke(); }
        if (Input.GetKeyDown(Skill)) { OnToggleSkill?.Invoke(); }
        if (Input.GetKeyDown(Quest)) { OnToggleQuest?.Invoke(); }
    }

}
