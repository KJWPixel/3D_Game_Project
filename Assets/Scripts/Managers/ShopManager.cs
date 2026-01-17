using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Playables;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [SerializeField] private GameObject shopPanel; //UICanvas 자식의 ShopPanel
    [SerializeField] private GameObject itemDetailPaenl;
    [SerializeField] private Transform itemListParent;
    [SerializeField] private GameObject itemSlotPrefab; // 아이템 슬롯 
    [SerializeField] private TextMeshProUGUI itemNameText; // 오른쪽 아아템 툴팁: 이름
    [SerializeField] private TextMeshProUGUI itemDescriptionText; // 설명
    [SerializeField] private TextMeshProUGUI itemPriceText; //가격
    [SerializeField] private Image itemIconImage; // 아이템 아이콘
    [SerializeField] private Button buyButton; //구매 버튼
    [SerializeField] private Button sellButton; //구매 버튼

    private List<ItemData> currentShopItems; // 현재 NPC의 상점 아이템 목록
    private ItemData selectItem; // 선택한 아이템
    private DialogNPC currentNPC; // 현재 상점 NPC
    private const string TABLE = "ITEM Table";

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

        if(shopPanel != null ) shopPanel.SetActive(false);
        if(itemDetailPaenl != null ) itemDetailPaenl.SetActive(false);

        buyButton.onClick.AddListener(OnBuyButtonClicked);
        sellButton.onClick.AddListener(OnSellButtonClicked);
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

        if(itemDetailPaenl != null) itemDetailPaenl.SetActive(false);
        PopuplateItemList(); // 왼쪽 목록 채우기
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

    private void PopuplateItemList()
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
                nameText.text = LocalizationSettings.StringDatabase.GetLocalizedString(TABLE, item.Itemkey);
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

        if(itemDetailPaenl != null) itemDetailPaenl.SetActive(true);

        // 아이템 이름 로컬라이제이션
        itemNameText.text = LocalizationSettings.StringDatabase.GetLocalizedString(TABLE, item.Itemkey);
        //itemNameText.text = item.name;

        // 아이템 타입에 따른 로컬라이제이션 매개변수값
        if (item.Type == ItemType.Equipment)
        {
            EquipementData equipData = item as EquipementData;
            object[] args = GetFormattedArgs(equipData);

            // 아이템 설명 로칼리이제이션
            itemDescriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString(TABLE, item.DescKey, args);
            //itemDescriptionText.text = item.Description;
        }
        else
        {
            // 아이템 설명 로칼리이제이션
            itemDescriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString(TABLE, item.DescKey);
            //itemDescriptionText.text = item.Description;
        }

        
        // 아이템 가격
        string priceLabel = LocalizationSettings.StringDatabase.GetLocalizedString("UI Table", "UI_PRICE");
        string goldLabel = LocalizationSettings.StringDatabase.GetLocalizedString("UI Table", "UI_GOLD");
        itemPriceText.text = $"{priceLabel} {item.Price} {goldLabel}";

        itemIconImage.sprite = item.Icon;
        itemIconImage.enabled = true;
        buyButton.interactable = true; // 구매 가능
    }

    // 오른쪽 초기화
    private void ClearItemDetail()
    {
        selectItem = null;

        if (itemDetailPaenl != null) itemDetailPaenl.SetActive(false);
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

        // 플레이어 Gold가 충분한지 ㅎ롹인
        if(PlayerStat.Instance.Gold < selectItem.Price)
        {
            Debug.Log("금액 부족");
            return;
        }

        // 인벤토리 추가 시도 (AddItem 내부에서 슬롯 꽉 참 여부 체크)
        // AddItem이 true를 반환하면성공, false면 슬롯 부족
        if(InventoryManager.Instance.AddItem(selectItem, 1))
        {
            // 추가에 성공했을 때만 돈을 차감
            PlayerStat.Instance.Gold -= selectItem.Price; // 금액 차감
            Debug.Log($"{selectItem.name} 구매 완료");
            //필요 시 UI 업데이트 또는 사운드
        }
        else
        {
            Debug.Log("구매 실패 (인벤토리 부족 등)");
        }
    }

    private void OnSellButtonClicked()
    {
        if (selectItem == null) return;

        // 장착 중인지 확인
        if(InventoryManager.Instance.IsItemEquipped(selectItem))
        {
            Debug.Log("장착 중인 아이템은 판매할 수 없습니다.");
            return;
        }

        // 인벤토리에서 제거 시도
        if(InventoryManager.Instance.RemoveItem(selectItem, 1))
        {
            // 판매 가격 계산 또는 ItemData.SellPrice 정의
            int sellPrice = Mathf.FloorToInt(selectItem.Price * 0.5f);
            PlayerStat.Instance.Gold += sellPrice;

            //SFX 돈 소리 추가 
            Debug.Log($"{selectItem} 판매완료 + {sellPrice} 골드");
        }
        else
        {
            Debug.Log("판매 실패: 인벤토리에 해당 아이템이 없습니다.");
        }
    }

    public bool IsShopOpen()
    {
        return shopPanel != null && shopPanel.activeInHierarchy;
    }

    private object[] GetFormattedArgs(EquipementData data)
    {
        if (data == null || data.EquipementStatus == null || data.EquipementStatus.Count == 0)
        {
            return new object[] { 0f };
        }

        float statValue = 0f;

        switch (data.EquipmentType)
        {
            case EquipmentType.Weapon:
            case EquipmentType.Helmet:
            case EquipmentType.Armor:
                statValue = data.EquipementStatus[0].Stat;
                break;
            case EquipmentType.Glove:
            case EquipmentType.Shoes:
                statValue = data.EquipementStatus[0].Stat * 100f;
                break;
        }

        return new object[] { statValue };
    }
}
