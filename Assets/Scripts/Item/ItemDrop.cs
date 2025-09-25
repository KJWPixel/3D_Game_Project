using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemDrop : MonoBehaviour
{
    //������ ��� ����
    //Enemy�� DropItem Table�� ������ ������ ������ Random���� ���ư�
    //�����۸�� Ȯ������ Ramdom�� ũ�ٸ� �ش� �����۸���� ItemData�� ���� DropItemPrefab�� �ش� ��ġ�� ����
    [System.Serializable]
    public class DropItem
    {
        public ItemData ItemData;
        public float DropProbability;
    }

    [Header("��� ������ ������")]
    [SerializeField] private GameObject DropItemPrefab;

    [Header("��� ������ ���")]
    [SerializeField] private List<DropItem> DropItems = new List<DropItem>();

    [Header("���� ��: ����ҋ� ���������� �ٲ�")]
    [SerializeField] private float RandomNumber = 0f;

    EnemyCharacter enemyCharacter;

    private void Awake()
    {
        enemyCharacter = GetComponent<EnemyCharacter>();
    }

    public void ItemsDrop()
    {
        if (enemyCharacter.IsDie && enemyCharacter != null)
        {
            RandomNumber = Random.Range(0f, 1f);
            Debug.Log($"RandomNumber ������ Ȯ��: {RandomNumber}");

            foreach (DropItem item in DropItems)
            {
                if (RandomNumber <= item.DropProbability)
                {
                    Debug.Log($"������ ���: {item.ItemData}");

                    GameObject DropObj = Instantiate(DropItemPrefab, Vector3.up, Quaternion.identity);
                    ItemPickup pickItem = DropObj.GetComponent<ItemPickup>();
                    pickItem.ItemData = item.ItemData;
                }
            }


        }
    }
}
