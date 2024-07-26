
using DataInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
   public static GameManger Ginstance;
   public bool isGameOver = false; 
   public Text Kill;
    public int killCount = 0;
    [SerializeField]
    public DataManger dataManger;
    [Header("DataManger")]
    [SerializeField]
    public GameData gameData;
    CanvasGroup inventoryOpen;

    // 인벤토리 아이템이 변경 되었을 때 발생 시킬 이벤트
    public delegate void ItemChageDelegate();
    public static event ItemChageDelegate OnItemChage;
    [SerializeField]
    private GameObject SlotList;
    public GameObject[] itemObject;
    void Awake()
    {
        if (Ginstance == null)
            Ginstance = this;
        else if (Ginstance != null)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
        dataManger = GetComponent<DataManger>();
        dataManger.Initialize();


        Kill = GameObject.Find("PlayerUi").transform.GetChild(7).GetComponent<Text>();
        inventoryOpen = GameObject.Find("Inventory").GetComponent<CanvasGroup>();
        InventoryOnOff(false);
        LoadGameDate();
    }
    public bool isPaused;
    public void Onpauseclear()
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused) ? 0 : 1;
        var playerobj = GameObject.FindGameObjectWithTag("Player");
        var scirpts = playerobj.GetComponents<MonoBehaviour>();
        foreach (var mono in scirpts)
        {
            mono.enabled = !isPaused;
        }
        var canvasGroup = GameObject.Find("Panel_Weapen").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
        

    }
    
    public void KillScroe()
    {
        ++gameData.killCount;
        Kill.text = $"<color=#0000000>Kill</color> :" + gameData.killCount.ToString("000");
        //PlayerPrefs.SetInt("KILLCOUNT", killCount);
        //킬수를 저장한다.
    }
    void InventorySetup()
    {
        var slots = SlotList.GetComponentsInChildren<Transform>(); // 자기 자식의 위치값을 다 가져옴
        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            for(int j = 1; j< slots.Length; j++)
            {
                // 아이템이 있으면 지나가라  (무시해라) 다른 아이템이 있으면 다음 인덱스로 바로 넘어감
                if (slots[j].childCount > 0) continue;
                // 보유한 아이템 종류에 따라 인덱스를 추출
                int itemIndex = (int)gameData.equipItem[i].itemtype;
                // 아이템의 부모는 Slots이 된다.
                itemObject[itemIndex].GetComponent<Transform>().SetParent(slots[j].transform);
                itemObject[itemIndex].GetComponent<ItemInfo>().itemdata = gameData.equipItem[i];
                //아이템의 ItemInfo 클래스의 Item데이터를 gmaeData.equipltem[i] 데이터 값을 저장

                break;
            }
        }
    }
    void LoadGameDate()
    {               // 플레이어의 프리퍼런스
                    //killCount = PlayerPrefs.GetInt("KILLCOUNT",0);
                    // 키값을 예약
        GameData data = dataManger.Load();
        gameData.hp = data.hp;
        gameData.damage = data.damage;
        gameData.speed = data.speed;
        gameData.equipItem = data.equipItem;
        gameData.killCount = data.killCount;
        Kill.text = $"<color=#ff0fff>Kill:</color>" +gameData.killCount.ToString("000");

     // 실무에서 쓴다고 하면 암호화 해서 저장 해야한다.
     if(gameData.equipItem.Count > 0)
        {
            InventorySetup();
        }
    }

    void SaveGameDate()
    {
        dataManger.Save(gameData);
    }
    // 만일 인벤토리에 아이템을 추가 했을 때 데이터 정보를 업데이트하는 함수
    public void AddItem(Item item)
    {
        // 만약 보유 아이템에 같은 아이템이면 추가하지 않고 빠져나감
        if (gameData.equipItem.Contains(item)) return;
        gameData.equipItem.Add(item);
        // 아이템을 GameData.item배열에 추가
        switch (item.itemtype)
        {
            case Item.ItemType.HP:
                // 아이템 계산 방식에 따라 처리
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp += item.valie;
                else
                    gameData.hp += gameData.hp * item.valie;

                break;
            case Item.ItemType.SPEED:
                // 아이템 계산 방식에 따라 처리
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed += item.valie;
                else
                    gameData.speed += gameData.speed * item.valie;

                break;
            case Item.ItemType.DAMAGE:
                // 아이템 계산 방식에 따라 처리
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage += item.valie;
                else
                    gameData.damage += gameData.damage * item.valie;

                break;
        }
        OnItemChage();
        // 아이템이 변경된 것을 실시간으로 반영 하기 위해 
        // 이벤트를 발생 시킴

       
    }
    public void RemoveItem(Item item)
    {
        gameData.equipItem.Remove(item);
        switch (item.itemtype)
        {
            case Item.ItemType.HP:
                // 아이템 계산 방식에 따라 처리
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp -= item.valie;
                else
                    gameData.hp = gameData.hp / item.valie;

                break;
            case Item.ItemType.SPEED:
                // 아이템 계산 방식에 따라 처리
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed -= item.valie;
                else
                    gameData.speed = gameData.speed / item.valie;

                break;
            case Item.ItemType.DAMAGE:
                // 아이템 계산 방식에 따라 처리
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage -= item.valie;
                else
                    gameData.damage = gameData.damage / item.valie;

                break;
        }
        OnItemChage();
    }

    private void OnApplicationQuit()
    {
        SaveGameDate();
    }
    private void OnDisable()
    {
        //PlayerPrefs.DeleteKey("KILLCOUNT");
    }
    public bool isOpened = false;
    public void InventoryButtonClick()
    {
        isOpened = !isOpened;
        InventoryOnOff(isOpened);
        
    }
    public void InventoryOnOff(bool isOpen)
    {
        inventoryOpen.alpha = isOpen ? 1f : 0f;
        inventoryOpen.interactable = isOpen ? true : false;
        inventoryOpen.blocksRaycasts = isOpen ? true : false;
    }

}
