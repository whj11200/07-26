using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo
{
    [System.Serializable]


    public class GameData // ����� ��Һ��ٴ� �������� ������ ���� Ŭ����                       
    {                     // �� �ȴ�. �̰��� Entity Ŭ������� �Ѵ�.


        public int killCount = 0;
        public float hp = 120f;
        public float damage = 25f;
        public float speed = 6.0f;
        public List<Item> equipItem = new List<Item>();


    }
    [System.Serializable]
    public class Item
    {
        public enum ItemType { HP,SPEED,GRENADE,DAMAGE}// ������ ���� ����
        public enum ItemCalc { VALUE, PERSENT} // ������ ��� ���
        public ItemType itemtype; 
        public ItemCalc itemCalc;
        public string name;// ������ �̸�
        public string desc; // ������ �Ұ�
        public float valie;// ��� ��

    }

}
