using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

    Stack<GameObject> panelStack = new Stack<GameObject>();
    [SerializeField] public GameObject Panel;
    [SerializeField] public Transform Parent;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        
    }

    public void ShowPopupUI()
    {
        
    }

    public void ClosePopupUI()
    {
        
    }

    public void UI_Option()
    {
        
    }

    public void OpnePopup()
    {
        GameObject Panels = Instantiate(Panel, Parent);
        panelStack.Push(Panels);
    }

    public void ClosePopup()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(panelStack.Count == 0)
            {
                return;
            }
            GameObject ClosePanel = panelStack.Pop();
            Destroy(ClosePanel);
        }
    }

}
