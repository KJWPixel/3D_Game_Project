using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

    [SerializeField] GameObject SkillWindow;
    [SerializeField] bool IsActiveCursor = false;


    Stack<GameObject> panelStack = new Stack<GameObject>();

    //[SerializeField] public GameObject Panel;
    //[SerializeField] public Transform Parent;


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

    private void Start()
    {
        UI_Initial();
    }

    private void UI_Initial()
    {
        SkillWindow.SetActive(false);
    }

    private void Update()
    {
        CursorActive();
        UI_SKillWIndow();
    }

    private void CursorActive()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            IsActiveCursor = !IsActiveCursor;
            
        }
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

    private void UI_SKillWIndow()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            if(SkillWindow.activeSelf)
            {
                SkillWindow.SetActive(false);
            }
            else
            {
                SkillWindow.SetActive(true);
            }
        }
    }

    

    //Stack을 이용한 Pop
    #region
    //public void OpnePopup()
    //{
    //    GameObject Panels = Instantiate(Panel, Parent);
    //    panelStack.Push(Panels);
    //}

    //public void ClosePopup()
    //{
    //    if(Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        if(panelStack.Count == 0)
    //        {
    //            return;
    //        }
    //        GameObject ClosePanel = panelStack.Pop();
    //        Destroy(ClosePanel);
    //    }
    //}
    #endregion 

}
