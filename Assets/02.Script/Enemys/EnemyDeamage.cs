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
    #region projectile ������� ����
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
    // �������� �޾����� �Լ��� ȣ�� 
    // object�迭�� ��������
    void OnDamage(object[] _parms)
    {
        // �Լ�ȣ���Ͽ� ��ġ���� 0��° ����
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

        // ���� ��ġ Collision ����ü�ȿ� Contacts��� �迭�� �ִ�.
        //Vector3 pos = other.contacts[0].point; //  ��ġ
        Vector3 pos = col; //  ��ġ
        Vector3 _nomal = col.normalized; // ����
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
