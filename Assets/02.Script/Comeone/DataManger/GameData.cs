using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo
{
    [System.Serializable]


    public class GameData // 기능적 요소보다는 데이터적 성격이 강한 클래스                       
    {                     // 가 된다. 이것을 Entity 클래스라고 한다.


        public int killCount = 0;
        public float hp = 120f;
        public float damage = 25f;
        public float speed = 6.0f;
        public List<Item> equipItem = new List<Item>();


    }
    [System.Serializable]
    public class Item
    {
        public enum ItemType { HP,SPEED,GRENADE,DAMAGE}// 아이템 종류 선언
        public enum ItemCalc { VALUE, PERSENT} // 아이템 계산 방식
        public ItemType itemtype; 
        public ItemCalc itemCalc;
        public string name;// 아이템 이름
        public string desc; // 아이템 소개
        public float valie;// 계산 값

    }

}
