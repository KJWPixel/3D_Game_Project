using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemDrop : MonoBehaviour
{
    //아이템 드랍 로직
    //Enemy는 DropItem Table을 가지고 있으며 죽으면 Random값이 돌아감
    //아이템목록 확률에서 Ramdom이 크다면 해당 아이템목록의 ItemData를 가진 DropItemPrefab을 해당 위치에 생성
    [System.Serializable]
    public class DropItem
    {
        public ItemData ItemData;
        public float DropProbability;
    }

    [Header("드랍 아이템 프리팹")]
    [SerializeField] private GameObject DropItemPrefab;

    [Header("드랍 아이템 목록")]
    [SerializeField] private List<DropItem> DropItems = new List<DropItem>();

    [Header("랜덤 값: 드랍할떄 랜덤값으로 바뀜")]
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
            Debug.Log($"RandomNumber 아이템 확률: {RandomNumber}");

            foreach (DropItem item in DropItems)
            {
                if (RandomNumber <= item.DropProbability)
                {
                    Debug.Log($"아이템 드랍: {item.ItemData}");

                    GameObject DropObj = Instantiate(DropItemPrefab, Vector3.up, Quaternion.identity);
                    ItemPickup pickItem = DropObj.GetComponent<ItemPickup>();
                    pickItem.ItemData = item.ItemData;
                }
            }


        }
    }
}
