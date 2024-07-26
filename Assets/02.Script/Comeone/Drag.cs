using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform ItemTr;
    private Transform inventoryTr;
    private Transform ItemList;
    private CanvasGroup canvasGroup;

    private string invenStr = "Inventory";
    private string itemListStr = "ItemList";

    public static GameObject draggingItem = null;
    void Start()
    {
        inventoryTr = GameObject.Find(invenStr).transform; //�θ�,�ڽĿ�����Ʈ�� �̺�Ʈ�� �߻��ϸ� �޶����⶧����
        ItemList = GameObject.Find(itemListStr).transform; //Find�� ã�´�.
        ItemTr = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnDrag(PointerEventData eventData) //�巡�� �̺�Ʈ (�巡���ϴ� �̺�Ʈ�� �߻��ϸ� ����)
    {
        ItemTr.position = Input.mousePosition; //�巡�� �̺�Ʈ�� �߻��ϸ� �������� ��ġ�� ���콺 Ŀ���� ��ġ�� �����Ѵ�.
    }

    public void OnBeginDrag(PointerEventData eventData)//�巡�׸� ������ �� �ѹ� ȣ�� �Ǵ� �̺�Ʈ
    {
        this.transform.SetParent(inventoryTr); //�巡�װ� ���۵Ǹ� �巡�� �޴� ������Ʈ�� �θ� ������Ʈ�� �κ��丮�� �ȴ�.
        draggingItem = this.gameObject; //�巡�׵� ������Ʈ�� draggingItem������ �ִ´�.
        canvasGroup.blocksRaycasts = false; //�巡�װ� ���۵Ǿ��� �� �ٸ� UI�̺�Ʈ�� ���� �ʱ� ����
    }

    public void OnEndDrag(PointerEventData eventData)//�巡�װ� ������ �� �ѹ� ȣ�� �Ǵ� �̺�Ʈ
    {
        draggingItem = null; //�巡�װ� �������Ƿ� ����� ������Ʈ�� ����
        canvasGroup.blocksRaycasts = true;
        if (ItemTr.parent == inventoryTr) //�巡���� �������� �θ� �κ��丮�϶�, �� �巡�װ� ������ �� ���Կ� ���� ���Ͽ��� ���
        {
            ItemTr.SetParent(ItemList);//�巡���� �������� �θ� ������ ����Ʈ�� �����Ѵ�. �ٽ� ������ ����Ʈ�� ���ư���.
            GameManger.Ginstance.RemoveItem(GetComponent<ItemInfo>().itemdata);
            //���Կ� �ִ� �������� ���� ��� ���ӸŴ������ִ� RemoveItem�Լ��� �ҷ��´�.
            //RemoveItem�Լ����� ���� �巡�� ���� �������� ������ itemData�� ��Ƽ� �����Ѵ�.
            //itemData�� GameData�� ItemŬ������ ������ �����̴�.
        }
        
    }
}
