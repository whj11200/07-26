using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
// 플레이가 되었을때 배럴 색상이 랜덤하게 만들기
// 5번 배럴이 총알에 폭파 물리현상 만들기
public class BarrelCtrl : MonoBehaviour
{
    [SerializeField]
    private Texture[] textures;
    [SerializeField]
    private MeshRenderer mesh;
    [SerializeField]
    private int HitCount = 0;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private string BulletTag = "Bullet";
    private string E_BulletTag = "E_Bullet";
    [SerializeField]
    private GameObject ExplosionPrefad;
    private AudioClip clip;
    private AudioClip BoomCilp;
    private string CameraTag = "MainCamera";
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private Mesh[] meshes;
    private GameObject effect2;


    private bool isExplo = false;
   

    void Start()
    {
        ExplosionPrefad = Resources.Load("EffectBoom") as GameObject;
        rb = GetComponent<Rigidbody>();
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        // textrues = Resources.LoadAll("")as <Texture>;
        mesh = GetComponent<MeshRenderer>();
        mesh.material.mainTexture = textures[Random.Range(0, textures.Length)];
        clip = Resources.Load("Sound/bullet_hit_metal_enemy_1") as AudioClip;
        BoomCilp = Resources.Load("Sound/grenade_exp2") as AudioClip;
        meshFilter = GetComponent<MeshFilter>();
        meshes = Resources.LoadAll<Mesh>("Meshes");
        effect2 = Resources.Load("DrippingFlames") as GameObject;


    }
    #region 프로젝타일의 방식의 충돌감지
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(BulletTag) || other.gameObject.CompareTag(E_BulletTag))
        {
            other.gameObject.SetActive(false);
            SoundManger.S_Instance.PlaySound(transform.position, clip);
            if (++HitCount == 5)
            {
                ExplosionBarrel();
                CamerSek.instance.TurnOn();

            }


        }

    }
    #endregion

    void OnDamage(object[] _params)
    {
        SoundManger.S_Instance.PlaySound(transform.position, clip);
       
        Vector3 firepos = (Vector3)_params[0];
        Vector3 hitpos = (Vector3)_params[1];
        // 맞은위치와 쏜위치의 거리를 구현
        Vector3 incomeVector = hitpos - firepos;
        // 전문용어로 입사 벡터라고 함
        incomeVector = incomeVector.normalized; // 입사벡터를 정규화 벡터로 변경
        var effect = Instantiate(effect2, hitpos, Quaternion.identity);
        Destroy(effect, 2f);
        // Ray 의 hit 좌표에 입사벡터의 각도로 힘을 생성
        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 1500f, hitpos);
        // 어떤 지점에 힘을 모아서  폭파가 생성되게 할때 호출 되는 메서드ㄴ
        if (++HitCount == 5 && !isExplo)
        {
            isExplo = true;
            ExplosionBarrel();
            CamerSek.instance.TurnOn();
            

        }
    }
    void ExplosionBarrel()
    {
        
        GameObject Effect = Instantiate(ExplosionPrefad,transform.position, Quaternion.identity);
        Destroy(Effect, 2f);
        Collider[] colls = Physics.OverlapSphere(transform.position, 20f, 1 << 7 | 1 << 13);
        // 배럴 자기자신 위치에서 20변경에 배럴 레이어만 Cols 라는 배열에 담는다.
      
        foreach (Collider coll in colls)
        {
            Rigidbody rigidbody = coll.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
               
                SoundManger.S_Instance.PlaySound(transform.position, BoomCilp);
                rigidbody.mass = 1.0f;
                rigidbody.AddExplosionForce(500, transform.position, 10f, 1000f);
                //Destroy(gameObject,2.0f);

                coll.gameObject.SendMessage("FlyingDie");
            }
            Invoke("BerralMassOrginal",1f);
         
            
          
            // 리지드바디 클래스 폭파 함수는 AddExplosionForce(폭파력,위치, 반경, 위로 솟구치는 힘)을 의미한다.
        }
        int ids = Random.Range(0, meshes.Length);
        // 터졋을시 메쉬필더를 sharedMesh하여 랜덤값으로 메쉬안에있는 mesh들이 변경되게 함
        meshFilter.sharedMesh = meshes[ids];
        GetComponent<MeshCollider>().sharedMesh = meshes[ids];
    }
    void BerralMassOrginal()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 100.0f );
        // 배럴 자기자신 위치에서 20변경에 배럴 레이어만 Cols 라는 배열에 담는다.

        foreach (Collider coll in colls)
        {
            Rigidbody rigidbody = coll.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                
                rigidbody.mass = 60f;
                

            }
           


           
        }

    }
  
}
