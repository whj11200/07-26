
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class EnemyAi : MonoBehaviour
{
    public enum State// ������ ���
    {
        PTROL = 0, TRACE, ATTACK, DIE,Dence,FlyingDead
    }
    public State state = State.PTROL;
    [SerializeField] private Transform Playertr; // �Ÿ��� ������� ����
    [SerializeField] private Transform Enemytr;  // �Ÿ��� ������� ����
    [SerializeField] private Animator animator; // �ִϸ�����
    // ���� �Ÿ� ���� �Ÿ�
    public float attackDist = 10.0f; // ���ݻ�Ÿ�
    public float traceDist = 10f;  // ���� ��Ÿ�
    public bool isDie = false; // �������
    private WaitForSeconds ws; // 
    private Enemy enemy;
    // �ִϸ����� ��Ʈ�ѷ��� ���� �� �Ķ������ �ؽð��� ������ �̸� ����
    private readonly int hashMove = Animator.StringToHash("Is_Move");
    private readonly int hashSpeed = Animator.StringToHash("MoveSpeed");
    private readonly int hashReload = Animator.StringToHash("Reload");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashIdx = Animator.StringToHash("DieIdx");
    private readonly int hashoffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashDance = Animator.StringToHash("Dance");
  

    private EnemyFire enemyFire;
    void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        enemyFire = GetComponent<EnemyFire>();
        var Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            Playertr = Player.GetComponent<Transform>();
        }
        Enemytr = GetComponent<Transform>();
        ws = new WaitForSeconds(0.3f);
    }
    private void OnEnable() //  ������Ʈ�� Ȱ��ȭ �ɶ� ȣ��
    {
        Deamge.OnPlayerDie += PlayerDie;
        // �̺�Ʈ ����
        animator.SetFloat(hashoffset, Random.Range(0.3f,1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(0.0f,5.0f));
        StartCoroutine(CacheState());
        StartCoroutine(Action());
    }
    IEnumerator CacheState()
    {
        while (!isDie)
        {
            if (state == State.DIE || state == State.FlyingDead) yield break;
            // ��� �����̸� �ڷ�ƾ �Լ��� ���� ��Ŵ
            float dist = (Playertr.position - Enemytr.position).magnitude;
            // ���� ���ݰŸ��� ���´ٸ�
            if (dist <= attackDist)
            {
                // ATTACKȰ��ȭ
                state = State.ATTACK;
            }
            // �߰ݰŸ��� ������
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PTROL;
            }
            yield return ws;
        }

    }
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;
            switch (state)
            {
                case State.PTROL:
                    enemyFire.isFire = false;
                    enemy.patroiing = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    enemyFire.isFire = true;
                    enemy.Stop();
                    animator.SetBool(hashMove, false);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    enemy.traceTaget = Playertr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.DIE:
                    EnemyDie();
                    break;
                case State.Dence:
                    PlayerDie();
                    break;
                case State.FlyingDead:
                    FlyingDead();
                    break;


            }
        }
        
    }

    private void EnemyDie()
    {
        
        enemy.Stop();
        enemyFire.isFire = false;
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashIdx, Random.Range(0, 3));
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        gameObject.tag = "Untagged";
        isDie = true;
        
        StartCoroutine(PoolPush());

    }
    IEnumerator PoolPush()
    {
        yield return new WaitForSeconds(3f);
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
        gameObject.tag = "Enemy"; //  ������Ʈ�� Ȱ��ȭ �Ǳ��� �±��̸��� �������
        gameObject.SetActive(false);
        state = State.PTROL;
        

    }
    void PlayerDie()
    {
        StopAllCoroutines();
        animator.SetTrigger(hashDance);
        GameManger.Ginstance.isGameOver = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;

    }
    void FlyingDead()
    {
        if (isDie) return;
        enemy.Stop();
        enemyFire.isFire = false;
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashIdx, 0);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        gameObject.tag = "Untagged";
        isDie = true;

        StartCoroutine(PoolPush());
    }

    void Update()
    {
        animator.SetFloat(hashSpeed,enemy.speed);
    }
    private void OnDisable()
    {
        Deamge.OnPlayerDie -= PlayerDie;
    }
}
