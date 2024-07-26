using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 800f;
    private Transform tr;
    private TrailRenderer trailRenderer;
    public float Damage = 25f;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = transform;
        trailRenderer = GetComponent<TrailRenderer>();

        Invoke("BulletDisavble",2.0f);
    }
    void BulletDisavble()
    {
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Damage = GameManger.Ginstance.gameData.damage;
        GameManger.OnItemChage += UpdateSetUP;
        rb.AddForce(tr.forward * speed);
    
    }
    
    void UpdateSetUP()
    {
       Damage = GameManger.Ginstance.gameData.damage;
    }
    private void OnDisable()
    {
        trailRenderer.Clear();
        //tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }


}
