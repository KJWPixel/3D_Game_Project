using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("UI Key ¼³Á¤")]
    public KeyCode Menu = KeyCode.Escape;
    public KeyCode Option = KeyCode.None;
    public KeyCode Inventory = KeyCode.I;
    public KeyCode Status = KeyCode.U;
    public KeyCode Skill = KeyCode.K;
    public KeyCode Quest = KeyCode.J;

    public KeyCode ItemSlot1 = KeyCode.F1;
    public KeyCode ItemSlot2 = KeyCode.F2;
    public KeyCode ItemSlot3 = KeyCode.F3;
    public KeyCode ItemSlot4 = KeyCode.F4;

    public event Action OnToggleMenu;
    public event Action OnToggleOption;
    public event Action OnToggleInventory;
    public event Action OnToggleStatus;
    public event Action OnToggleSkill;  
    public event Action OnToggleQuest;
    public event Action OnUseItem;

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
        if (Input.GetKeyDown(Menu)) { OnToggleMenu?.Invoke();  }
        if (Input.GetKeyDown(Option)) { OnToggleOption?.Invoke(); }
        if (Input.GetKeyDown(Inventory)) { OnToggleInventory?.Invoke(); }
        if (Input.GetKeyDown(Status)) { OnToggleStatus?.Invoke(); }
        if (Input.GetKeyDown(Skill)) { OnToggleSkill?.Invoke(); }
        if (Input.GetKeyDown(Quest)) { OnToggleQuest?.Invoke(); }

        if (Input.GetKeyDown(KeyCode.F1))
            UI_ItemSlotManager.Instance.GetSlot(0)?.UseItem();

        if (Input.GetKeyDown(KeyCode.F2))
            UI_ItemSlotManager.Instance.GetSlot(1)?.UseItem();

        if (Input.GetKeyDown(KeyCode.F3))
            UI_ItemSlotManager.Instance.GetSlot(2)?.UseItem();

        if (Input.GetKeyDown(KeyCode.F4))
            UI_ItemSlotManager.Instance.GetSlot(3)?.UseItem();
    }

}
