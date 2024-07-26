using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Common.WebApi.WebApiEndpoints;
using UnityEngine.UI;

public class Deamge : MonoBehaviour
{
    private readonly string E_BulletTag = "E_Bullet";
    public GameObject Blood;
    [SerializeField]
    private int curhp = 100;
    [SerializeField]
    private int inithp = 100;
    private bool isDie = false;
    private Rigidbody rb;
    private CapsuleCollider cp;
    
    public Image Damege;
    [SerializeField]
    private Image HpBar;
    [SerializeField]
    private Text HpText;

    public delegate void PlayerDie();
    public static event PlayerDie OnPlayerDie;

    private void OnEnable()
    {
        GameManger.OnItemChage += UpdateSetUP;
        // �̺�Ʈ ���
    }
    void UpdateSetUP()
    {
        inithp = (int)GameManger.Ginstance.gameData.hp;
        curhp += (int)GameManger.Ginstance.gameData.hp - curhp;
    }

    void Start()
    {
        // inithp = 
        inithp = (int)GameManger.Ginstance.gameData.hp;
        curhp = inithp;
        HpBar = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(2).GetComponent<Image>();
        HpText = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(0).GetComponent<Text>();
        Damege = GameObject.Find("PlayerUi").transform.GetChild(0).GetComponent<Image>();
        Blood = Resources.Load("Effects/BulletImpactFleshSmallEffect") as GameObject;
        rb = GetComponent<Rigidbody>();
        cp = rb.GetComponent<CapsuleCollider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(E_BulletTag))
        {
            collision.gameObject.SetActive(false);

            // ���� ��ġ Collision ����ü�ȿ� Contacts��� �迭�� �ִ�.
            GameObject blood = ShowBloodEffect(collision);

            curhp -= 10;
            HpBarText();

            if (curhp <= 0)
            {
                curhp = Mathf.Clamp(curhp, 0, inithp);
                Debug.Log("��������");
                PlayersDie();
            }
            StartCoroutine(showBloodScreen());
        }

    }

    private void HpBarText()
    {
        HpBar.fillAmount = (float)curhp / (float)inithp;
        if (curhp ==50)
            HpBar.color = Color.yellow;
        else if (curhp == 20)
        {
             HpBar.color = Color.red;
        }
        HpText.text = $"<color=#ff0000>{curhp}</color>/{inithp}";
        if (curhp == 50)
            HpText.color = Color.yellow;
        else if (curhp == 20)
            HpText.color = Color.black;
    }

    IEnumerator showBloodScreen()
    {
        Damege.color = new Color(1, 0, 0, Random.Range(0.25f, 0.35f));
        yield return new WaitForSeconds(0.1f);
        Damege.color = Color.clear;
    }

    private GameObject ShowBloodEffect(Collision collision)
    {
        Vector3 pos = collision.contacts[0].point; //  ��ġ
        Vector3 _nomal = collision.contacts[0].normal; // ����
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _nomal);
        GameObject blood = Instantiate(Blood, pos, rot);
        Destroy(blood, 1.0f);
        return blood;
    }

    public void PlayersDie()
    {
        #region ���� ������ ���� MMORPG�� ������ �ʴ�.
        //isDie = true;

        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //for (int i = 0; i< enemies.Length; i++)
        //{
        //    enemies[i].gameObject.SendMessage("PlayerDie",SendMessageOptions.DontRequireReceiver);
        //}
        #endregion
        OnPlayerDie();


    }
}
