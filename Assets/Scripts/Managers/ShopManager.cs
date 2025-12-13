using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [SerializeField] private GameObject shopPanel; //UICanvas 자식의 ShopPanel
    [SerializeField] private Transform itemListParent;
    [SerializeField] private GameObject itemSlotPrefab; // 아이템 슬롯 
    [SerializeField] private TextMeshProUGUI itemNameText; // 오른쪽 아아템 툴팁: 이름
    [SerializeField] private TextMeshProUGUI itemDescriptionText; // 설명
    [SerializeField] private TextMeshProUGUI itemPriceText; //가격
    [SerializeField] private Image itemIconImage; // 아이템 아이콘
    [SerializeField] private Button buyButton; //구매 버튼

    private List<ItemData> currentShopItems; // 현재 NPC의 상점 아이템 목록
    private ItemData selectItem; // 선택한 아이템
    private DialogNPC currentNPC; // 현재 상점 NPC

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        shopPanel.SetActive(false);
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    private void Update()
    {
        if (shopPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }

    // DialogueManager의 EndDialogue에서 호출 
    public void OpenShop(DialogNPC npc)
    {
        currentNPC = npc;
        currentShopItems = LoadShopItems(npc); //NPC별 아이템 목록 로드
        shopPanel.SetActive(true);
        PopulateItemList(); // 왼쪽 목록 채우기
        ClearItemDetail();  // 오륹쪽 초기화
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        currentShopItems = null;
        selectItem = null;
    }

    // NPC별 상점 아이템 로드 (ScriptableObject나 데이터베이스에서 로드)
    private List<ItemData> LoadShopItems(DialogNPC npc)
    {
        if (npc == null || npc.itemDatas == null)
            return new List<ItemData>();

        return npc.itemDatas;
    }

    private void PopulateItemList()
    {
        //기존 슬롯 삭제
        foreach (Transform child in itemListParent)
        {
            Destroy(child.gameObject);
        }

        foreach(ItemData item in currentShopItems)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemListParent);
            //슬롯 UI 설정 (예: Text = item.name, Image = item.Icon)

            // 아이템 이름 텍스트 찾기
            TextMeshProUGUI nameText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = item.ItemName;
            }
            else
            {
                Debug.LogWarning("itemSlotPrefab에 TextMeshProUGUI가 없습니다!", itemSlotPrefab);
            }


            // 아이콘 찾기
            Image iconImage = slot.transform.Find("ItemIcon")?.GetComponent<Image>();
            if (iconImage != null && item.Icon != null)
            {
                iconImage.sprite = item.Icon;
            }
                
            // 버튼 찾기
            Button button = slot.GetComponentInChildren<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners(); // 중요! 중복 방지
                button.onClick.AddListener(() => SelectItem(item));
            }
            else
            {
                Debug.LogError("ShopItemSlotPrefab에 Button 컴포넌트가 없습니다!", slot);
            }
        }
    }

    // 아이템 선택 시 오른쪽 툴팁 업데이트
    private void SelectItem(ItemData item)
    {
        selectItem = item;
        itemNameText.text = item.name;
        itemDescriptionText.text = item.Description;
        itemPriceText.text = $"가격: {item.Price} Gold";
        itemIconImage.sprite = item.Icon;
        itemIconImage.enabled = true;

        buyButton.interactable = true; // 구매 가능
    }

    // 오른쪽 초기화
    private void ClearItemDetail()
    {
        selectItem = null;
        itemNameText.text = null;
        itemDescriptionText.text = null;
        itemPriceText.text = null;
        itemIconImage.sprite = null;
        buyButton.interactable = false;

        itemIconImage.enabled = false;
    }

    // 구매 버튼 클릭
    private void OnBuyButtonClicked()
    {
        if(selectItem == null) return;

        //플레이어 금액 확인
        if(PlayerStat.Instance.Gold < selectItem.Price)
        {
            Debug.Log("금액 부족");
            return;
        }

        //인벤토리 추가 (기존 InventoryManager 사용)
        if(InventoryManager.Instance.AddItem(selectItem, 1))
        {
            PlayerStat.Instance.Gold -= selectItem.Price; // 금액 차감
            Debug.Log($"{selectItem.name} 구매 완료");
            //필요 시 UI 업데이트 또는 사운드
        }
        else
        {
            Debug.Log("구매 실패 (인벤토리 부족 등)");
        }
    }

    public bool IsShopOpen()
    {
        return shopPanel != null && shopPanel.activeInHierarchy;
    }
}
