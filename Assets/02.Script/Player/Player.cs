using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;
using UnityEngine.EventSystems;
// ��Ʃ����Ʈ public ����� ��� �ʵ带
[System.Serializable]
public struct PlayerSound
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}
[System.Serializable]
public class Playeranimation // �ν��Ͻ� â�� ���� �ش�.
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runLeft;
    public AnimationClip runRight;
    public AnimationClip Sprint;

}

public class Player : MonoBehaviour
{
    public enum WeapenType
    {
        RIFLE = 1, SHOTGUN =2
    }
    public WeapenType curType = WeapenType.SHOTGUN;
    
    public PlayerSound playersound;
    public Playeranimation playeranimation;
    [SerializeField]
    private float movespeed = 5f;
    [SerializeField]
    private float rotspeed = 250f;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    CapsuleCollider capsule;
    [SerializeField]
    Transform tr;
    [SerializeField]
    Animation _animation;
    float h, v, x;
    [SerializeField]
    private Transform firepos;
    public ParticleSystem muzzle;
    private AudioSource source;
    public AudioClip clip;
    [SerializeField]
    private GameObject A4A1;
    [SerializeField]
    private GameObject ShotGun;
    [SerializeField]
    public Image magazineImg; // źâ�̹���
    public Text magazineText; // ���� �Ѿ��� ��
    public float reloadTime = 2.0f; //������ �ð�
    public bool is_reload = false;
    private bool DontFire=false;
    private string E_Bullet = "E_Bullet";
    private readonly string EnemyTag = "Enemy";
    private readonly string BarrelTag = "Barrel";
    private readonly string WallTag = "Wall";
    private const float Dist = 20f;
    public int maxBullet = 10;
    public int remaingBullet = 10;
    public Sprite[] weaponIcons;
    public Image weaponImg;
    private void OnEnable()
    {
        GameManger.OnItemChage += UpdateSetUP;
        // �̺�Ʈ ���
    }
    void UpdateSetUP()
    {
        movespeed = GameManger.Ginstance.gameData.speed;
    }
    void Start()
    {
        movespeed = GameManger.Ginstance.gameData.speed;
        // ���۳�Ʈ ĳ�� ó��
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();  
        _animation = GetComponent<Animation>();
        tr = GetComponent<Transform>();
        _animation.Play(playeranimation.idle.name);
        
        firepos = GameObject.Find("Player").transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0);
        clip = Resources.Load("Sound/p_m4_1") as AudioClip;   
        source = GetComponent<AudioSource>();
        muzzle.Stop();
        magazineImg = GameObject.Find("PlayerUi").transform.GetChild(1).GetChild(2).GetComponent<Image>();
        magazineText = GameObject.Find("PlayerUi").transform.GetChild(1).GetChild(0).GetComponent<Text>();
        weaponIcons = Resources.LoadAll<Sprite>("WeaponIcons");
        weaponImg =  GameObject.Find("PlayerUi").transform.GetChild(3).GetChild(0).GetComponent<Image>();
    }


    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
           // Ui�� Ư�� �̺�Ʈ�� �߻� �Ǹ� ���� ������.
        //Debug.DrawLine(firepos.position, firepos.forward * 100f, Color.red);
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        x = Input.GetAxisRaw("Mouse X");
        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir.normalized * movespeed * Time.deltaTime, Space.Self);
        MoveAnimation();
        Runing();
        tr.Rotate(Vector3.up * x * Time.deltaTime * rotspeed);
        if (DontFire == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                --remaingBullet;
                muzzle.Play();
                Invoke("turnOFF",2f);
                Fire();
                if(remaingBullet == 0)
                {
                    
                    StartCoroutine(Reloaing());
                }
                
            }
        }
      
        

    }

    

    IEnumerator Reloaing()
    {
        is_reload = true;
        SoundManger.S_Instance.PlaySound(transform.position, playersound.reload[(int)curType]);
        yield return
             new WaitForSeconds(playersound.reload[(int)curType].length +0.3f);
        is_reload = false;
        magazineImg.fillAmount = 1.0f;
        remaingBullet = maxBullet;
        UpdateBulletTest();
    }
    public void OnChangeWeapon()
    {
        
        curType = (WeapenType)((int)++curType % 2);
        weaponImg.sprite = weaponIcons[(int)curType];
        // ��������Ʈ �̹�����ü
          
       
    }
    private void Fire()
    {
        #region projectctile movement ���

        //var _bullet = ObjectPullingManger.pullingManger.GetBulletPool();
        //if (_bullet != null)
        //{
        //    _bullet.transform.position = firepos.position;
        //    _bullet.transform.rotation = firepos.rotation; 
        //    _bullet.SetActive(true);
        //}
        //

        ////var firebullet = Instantiate(bullet, firepos.position, firepos.rotation);
        #endregion
        RaycastHit hit;// ������ ������Ʈ�� �浹�����̳�
                       // �Ÿ����� �˷��ִ� ���� ����ü
                       // ������ �i���� �ش���ġ  �ش� ���� ����� 
        if (Physics.Raycast(firepos.position, firepos.forward, out hit, Dist))
        {
            if (hit.collider.CompareTag(EnemyTag) )
            {
          
                object[] _parms = new object[3];
                _parms[0] = hit.point; // ù��° �迭�� ���� ��ġ�� ����,������ġ
                _parms[1] = 25f;//��������
              


                // ������ ���� ������Ʈ�� �Լ��� ȣ���ϸ鼭 �Ű����� ���� ����
                hit.collider.gameObject.SendMessage("OnDamage", _parms, SendMessageOptions.DontRequireReceiver);
            }
            if (hit.collider.CompareTag(BarrelTag))
            {
                object[] _parms = new object[2];
                _parms[0] = firepos.position;// �߻���ġ
                _parms[1] = hit.point; // ù��° �迭�� ���� ��ġ�� ����,������ġ

                hit.collider.gameObject.SendMessage("OnDamage", _parms, SendMessageOptions.DontRequireReceiver);
            }
            if (hit.collider.CompareTag(WallTag))
            {
                object[] _parms = new object[2];
                _parms[0] = hit.point; // ù��° �迭�� ���� ��ġ�� ����,������ġ
                _parms[1] = firepos.position;// �߻���ġ
                hit.collider.gameObject.SendMessage("OnDamage", _parms, SendMessageOptions.DontRequireReceiver);
            }




        }
        source.PlayOneShot(clip, 1.0f);
        UpdateBulletTest();
    }

    private void UpdateBulletTest()
    {
        magazineImg.fillAmount = (float)remaingBullet / (float)maxBullet;
        magazineText.text = string.Format("<color=#ff0000>{0}</color> / {1}", remaingBullet, maxBullet);
    }

    private void Runing()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) )
        {
            movespeed = GameManger.Ginstance.gameData.speed*2;
            _animation.CrossFade(playeranimation.Sprint.name, 0.3f);
            DontFire = true;
            muzzle.Stop();
        }
        else if (Input.GetKey(KeyCode.D)&& Input.GetKey(KeyCode.A)&& Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {
            movespeed = GameManger.Ginstance.gameData.speed*2;
            muzzle.Stop();
            DontFire = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movespeed = GameManger.Ginstance.gameData.speed;
            DontFire = false;
            muzzle.Stop();
        }
    }

    private void MoveAnimation()
    {
        if (h > 0.1f)
        {
            _animation.CrossFade(playeranimation.runRight.name, 0.3f);
            // ���� ���� Ŭ���� �� ���� ����Ŭ�� �ִϸ��̼� 0.3�ʰ� ȥ���ϸ� �ε巯�� �ִϸ��̼��� ���´�.

        }
        else if (h < -0.1f)
        {
            _animation.CrossFade(playeranimation.runLeft.name, 0.3f);
        }
        else if (v > 0.1)
        {
            _animation.CrossFade(playeranimation.runForward.name, 0.3f);
        }
        else if (v < -0.1)
        {
            _animation.CrossFade(playeranimation.runBackward.name, 0.3f);
        }
        else if (h == 0 && v == 0)
        {
            _animation.CrossFade(playeranimation.idle.name);
        }
    }
    void turnOFF()
    {
        muzzle.Stop();
    }
    
}
