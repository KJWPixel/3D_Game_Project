using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    Queue<string> Que = new Queue<string>();

    [SerializeField] TextMeshPro Text;
    Color Color;
    Outline EffectColor;
    [SerializeField] float TextSpeed = 0f;
    [SerializeField] float TextDestroyTime = 0f;

    private void Start()
    {
        Que.Enqueue("Queue");
        Que.Enqueue("12345");
        Que.Enqueue("ABCDE");


        StartCoroutine(QueueOut());
    }

    void Update()
    {
    
    }

    public void SetDamageText(float _Damage)
    {
        Text.text = _Damage.ToString();
    }

    private void TextMove()
    {
        Text.transform.position = Vector3.up;
    }


    private void Enqueue(string _string)
    {
        Que.Enqueue(_string);
    }

    private void Dequeue()
    {
        Que.Dequeue();
    }

    IEnumerator QueueOut()
    {
        while (Que.Count > 0) 
        {
            string str = Que.Dequeue();
            Debug.Log(str);
            yield return new WaitForSeconds(3);
        }             
    }
}
