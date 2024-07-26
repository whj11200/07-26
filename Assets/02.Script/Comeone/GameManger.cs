
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

    // �κ��丮 �������� ���� �Ǿ��� �� �߻� ��ų �̺�Ʈ
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
        //ų���� �����Ѵ�.
    }
    void InventorySetup()
    {
        var slots = SlotList.GetComponentsInChildren<Transform>(); // �ڱ� �ڽ��� ��ġ���� �� ������
        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            for(int j = 1; j< slots.Length; j++)
            {
                // �������� ������ ��������  (�����ض�) �ٸ� �������� ������ ���� �ε����� �ٷ� �Ѿ
                if (slots[j].childCount > 0) continue;
                // ������ ������ ������ ���� �ε����� ����
                int itemIndex = (int)gameData.equipItem[i].itemtype;
                // �������� �θ�� Slots�� �ȴ�.
                itemObject[itemIndex].GetComponent<Transform>().SetParent(slots[j].transform);
                itemObject[itemIndex].GetComponent<ItemInfo>().itemdata = gameData.equipItem[i];
                //�������� ItemInfo Ŭ������ Item�����͸� gmaeData.equipltem[i] ������ ���� ����

                break;
            }
        }
    }
    void LoadGameDate()
    {               // �÷��̾��� �����۷���
                    //killCount = PlayerPrefs.GetInt("KILLCOUNT",0);
                    // Ű���� ����
        GameData data = dataManger.Load();
        gameData.hp = data.hp;
        gameData.damage = data.damage;
        gameData.speed = data.speed;
        gameData.equipItem = data.equipItem;
        gameData.killCount = data.killCount;
        Kill.text = $"<color=#ff0fff>Kill:</color>" +gameData.killCount.ToString("000");

     // �ǹ����� ���ٰ� �ϸ� ��ȣȭ �ؼ� ���� �ؾ��Ѵ�.
     if(gameData.equipItem.Count > 0)
        {
            InventorySetup();
        }
    }

    void SaveGameDate()
    {
        dataManger.Save(gameData);
    }
    // ���� �κ��丮�� �������� �߰� ���� �� ������ ������ ������Ʈ�ϴ� �Լ�
    public void AddItem(Item item)
    {
        // ���� ���� �����ۿ� ���� �������̸� �߰����� �ʰ� ��������
        if (gameData.equipItem.Contains(item)) return;
        gameData.equipItem.Add(item);
        // �������� GameData.item�迭�� �߰�
        switch (item.itemtype)
        {
            case Item.ItemType.HP:
                // ������ ��� ��Ŀ� ���� ó��
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp += item.valie;
                else
                    gameData.hp += gameData.hp * item.valie;

                break;
            case Item.ItemType.SPEED:
                // ������ ��� ��Ŀ� ���� ó��
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed += item.valie;
                else
                    gameData.speed += gameData.speed * item.valie;

                break;
            case Item.ItemType.DAMAGE:
                // ������ ��� ��Ŀ� ���� ó��
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage += item.valie;
                else
                    gameData.damage += gameData.damage * item.valie;

                break;
        }
        OnItemChage();
        // �������� ����� ���� �ǽð����� �ݿ� �ϱ� ���� 
        // �̺�Ʈ�� �߻� ��Ŵ

       
    }
    public void RemoveItem(Item item)
    {
        gameData.equipItem.Remove(item);
        switch (item.itemtype)
        {
            case Item.ItemType.HP:
                // ������ ��� ��Ŀ� ���� ó��
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp -= item.valie;
                else
                    gameData.hp = gameData.hp / item.valie;

                break;
            case Item.ItemType.SPEED:
                // ������ ��� ��Ŀ� ���� ó��
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed -= item.valie;
                else
                    gameData.speed = gameData.speed / item.valie;

                break;
            case Item.ItemType.DAMAGE:
                // ������ ��� ��Ŀ� ���� ó��
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
