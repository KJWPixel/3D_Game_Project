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
        [Range(0f, 1f)]
        public float DropProbability;
    }

    [Header("드랍 아이템 프리팹(하나의 오브젝트에 여러 아이템을 담음)")]
    [SerializeField] private GameObject DropItemPrefab;

    [Header("드랍 아이템 목록")]
    [SerializeField] private List<DropItem> DropItems = new List<DropItem>();

    EnemyCharacter enemyCharacter;

    private void Awake()
    {
        enemyCharacter = GetComponent<EnemyCharacter>();
    }

    // 적이 죽었을 떄 호출되는 드랍함수
    public void ItemsDrop()
    {
        if (enemyCharacter == null || !enemyCharacter.IsDie) return;

        // 실제로 드랍될 아이템
        List<ItemData> droppedItems = new List<ItemData>();

        // 각 아이템마다 독립적인 확률 판단
        foreach(DropItem drop in DropItems)
        {
            if(drop.ItemData == null) continue;

            float rand = Random.Range(0f, 1f);
            if(rand <= drop.DropProbability)
            {
                droppedItems.Add(drop.ItemData);
                Debug.Log($"아이템 드랍됨: {drop.ItemData.Itemkey} (확률: {drop.DropProbability})");
            }
        }

        if(droppedItems.Count > 0)
        {
            GameObject dropObj = Instantiate(DropItemPrefab,
            transform.position + Vector3.up * 0.5f, Quaternion.identity);

            ItemPickup pickup = dropObj.GetComponent<ItemPickup>();
            if(pickup != null)
            {
                pickup.SetItems(droppedItems);
                Debug.Log($"총 {droppedItems.Count}개의 아이템이 하나의 드랍 오브젝트에 담겼습니다.");
            }
            else
            {
                Debug.LogError("DropItemPrefab에 ItemPickup 컴포넌트가 없습니다!");
            }
        }
        else
        {
            Debug.Log("아이템이 드랍되지 않았습니다.");
        }



        //if (enemyCharacter.IsDie && enemyCharacter != null)
        //{
        //    RandomNumber = Random.Range(0f, 1f);
        //    Debug.Log($"RandomNumber 아이템 확률: {RandomNumber}");

        //    foreach (DropItem item in DropItems)
        //    {
        //        if (RandomNumber <= item.DropProbability)
        //        {
        //            DroppedItem.Add(item.ItemData);
        //            Debug.Log($"아이템 드랍: {item.ItemData}");
        //        }
        //    }

        //    GameObject DropObj = Instantiate(DropItemPrefab, enemyCharacter.transform.position + Vector3.up, Quaternion.identity);
        //    ItemPickup pickItem = DropObj.GetComponent<ItemPickup>();
        //    pickItem.SetItems(DroppedItem);
        //}
    }
}
