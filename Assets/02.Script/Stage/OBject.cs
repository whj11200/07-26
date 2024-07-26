using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBject : MonoBehaviour
{
    private Transform tr;
    public GameObject effect2;
    private string Bullettag = "Bullet";
    private string E_Bullettag = "E_Bullet";

    private AudioSource source;

    private AudioClip clip;
    void Start()
    {
        tr = transform;
        source = GetComponent<AudioSource>();
        clip = Resources.Load("Sound/bullet_hit_metal_enemy_1") as AudioClip;
        effect2 = Resources.Load("FlareMobile") as GameObject;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Bullettag)||collision.gameObject.CompareTag(E_Bullettag))
        {
            collision.gameObject.SetActive(false);
            
            var effect =Instantiate(effect2,collision.transform.position,collision.transform.rotation);
            Destroy(effect, 2f);
            SoundManger.S_Instance.PlaySound(transform.position, clip);
        }
    }
    void OnDamage(object[] _parms)
    {
        Vector3 hitpos = (Vector3)_parms[0];
        //Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hitpos.normalized);
        var effect = Instantiate(effect2,hitpos,Quaternion.identity);
        Destroy(effect, 2f);
        SoundManger.S_Instance.PlaySound(hitpos, clip);
    }
   
}
