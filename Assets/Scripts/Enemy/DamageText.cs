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

    void Update()
    {
          
    }

    void LateUpdate()
    {
        transform.forward = MainCamera.transform.forward;

    }

    public void SetDamageText(float _Damage, bool _IsCritical)
    {
        Text.text = _Damage.ToString();
        Text.color = Color.white;
        if(_IsCritical)
        {
            Text.color += Color.red;
        }

        Animator.SetTrigger("TextPlay");    
    }
}
