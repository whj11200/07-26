using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class EnemyDeamage : MonoBehaviour
{
    [SerializeField]
    private readonly string BulletTag = "Bullet";
    [SerializeField]
    public GameObject BloodEffect;
    [SerializeField]
    private float Hp = 100;
    void Start()
    {
        BloodEffect = Resources.Load("Effects/BulletImpactFleshSmallEffect") as GameObject;
        
    }
    #region projectile 방식으로 감지
    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.collider.CompareTag(BulletTag))
    //    {
    //        other.gameObject.SetActive(false);
    //        ShowBloodEffect(other);
    //        Hp -= other.gameObject.GetComponent<Bullet>().Damage;
    //        Hp = Mathf.Clamp(Hp, 0f, 100f);
    //        if (Hp <= 0f)
    //        {
    //            Die();
    //        }
    //    }
    //}
    #endregion
    // 데미지를 받았을때 함수를 호출 
    // object배열에 변수생성
    void OnDamage(object[] _parms)
    {
        // 함수호출하여 위치값을 0번째 지정
        ShowBloodEffect((Vector3)_parms[0]);
        Hp -= (float)_parms[1];
        Hp = Mathf.Clamp(Hp, 0f, 100f);
        if (Hp <= 0f)
        {
            Die();
        }
    }

    private void ShowBloodEffect(Vector3 col)
    {

        // 맞은 위치 Collision 구조체안에 Contacts라는 배열이 있다.
        //Vector3 pos = other.contacts[0].point; //  위치
        Vector3 pos = col; //  위치
        Vector3 _nomal = col.normalized; // 방향
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, _nomal);
        GameObject blood = Instantiate(BloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
    void Die()
    {

        GetComponent<EnemyAi>().state = EnemyAi.State.DIE;
        GameManger.Ginstance.KillScroe();
        
    }
    void FlyingDie()
    {
        GetComponent<EnemyAi>().state = EnemyAi.State.FlyingDead;
        GameManger.Ginstance.KillScroe();
    }
}
