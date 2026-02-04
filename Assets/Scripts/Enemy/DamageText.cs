using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    Queue<string> Que = new Queue<string>();

    [SerializeField] TextMeshPro Text;
    [SerializeField] float TextSpeed = 0f;
    [SerializeField] float TextDestroyTime = 0f;

    private Camera MainCamera;

    Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        MainCamera = Camera.main;

        Destroy(gameObject, TextDestroyTime);
    }

    void LateUpdate()
    {
        transform.forward = MainCamera.transform.forward;
    }

    public void SetDamageText(float damage, bool isCritical)
    {
        Text.text = damage.ToString("F0");
      
        if(isCritical)
        {
            Text.color = Color.red;
            Text.fontSize *= 1.2f;
            Text.text += "\nCritical";
        }
        else
        {
            Text.color = Color.white;
        }

        Animator.SetTrigger("TextPlay");
    }
}
