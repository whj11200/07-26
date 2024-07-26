using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPullingManger : MonoBehaviour
{
    public static ObjectPullingManger pullingManger;
    [SerializeField]
    
    private GameObject bullet; // 
    private GameObject E_bullet;
    public int maxPool = 10;// 오브젝트 풀링 할 갯수
    // List 어떤 자료형에 따라 담을 수 있을 것 배열은 뭐든 담는것
    public List<GameObject> bulletPoolList;
    public List <GameObject> E_bulletPoolList;
    [Header("EnemyObjectPool")]
    public GameObject EnemyPrefad;
    public List<GameObject> EnemyPoolList;
    public List<Transform> SpwanPointsList;

   
    private void Awake() // Start,Awake,OnEnable 다같이 시작하자마자 호출 
    {
        if (pullingManger == null)
        {
            pullingManger = this;
        }
        else if (pullingManger != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        bullet = Resources.Load("Bullet") as GameObject;
        E_bullet = Resources.Load("EnBullt") as GameObject;
        EnemyPrefad = Resources.Load("Enemy") as GameObject;
        CreateBulletPool();
        CrateBulletPoolEnemy();
        CreatEnemyPull();
    }

    private void CreatEnemyPull()
    {
        GameObject EnemyGroup = new GameObject("EnemyGroup");
        
        for (int i = 0; i < maxPool; i++)
        {
            var enemyObj = Instantiate(EnemyPrefad, EnemyGroup.transform);
            enemyObj.name = $"{(i + 1).ToString()} 명";
            enemyObj.SetActive(false);
            EnemyPoolList.Add(enemyObj);
        }
    }

    void CreateBulletPool()
    {
        // 게임 오브젝트 생성
        GameObject playerBulletGroup = new GameObject("PlayerBulletGroup");
        for( int i = 0; i< 10; i++ )
        {
            var _bullet = Instantiate(bullet,playerBulletGroup.transform);
            _bullet.name = $"{(i+1).ToString()}발";
            _bullet.SetActive(false);
            bulletPoolList.Add(_bullet);
        }
    }
    void CrateBulletPoolEnemy()
    {
        GameObject EnemyBulletGroup = new GameObject("EnemyGroup");
        for (int i = 0; i < 10; i++)
        {
            var _bullet2 = Instantiate(E_bullet, EnemyBulletGroup.transform);
            _bullet2.name = $"{(i + 1).ToString()}발";
            _bullet2.SetActive(false);
            E_bulletPoolList.Add(_bullet2);
        }
    }
  
    public GameObject CreatEnBulletPool()
    {
        for (int i = 0; i< E_bulletPoolList.Count; i ++ )
        {
            if ( E_bulletPoolList[i].activeSelf == false)
            {
                return E_bulletPoolList[i];
            }
        }
        return null;
    }
    public GameObject GetBulletPool()
    {
        for(int i = 0; i< bulletPoolList.Count; i++ )
        {
            // 비활성 되었다면 activeSelf는 활성화 비활성 여부를 알려줌
            if (bulletPoolList[i].activeSelf==false)
            {
                return bulletPoolList[i];
            }
        }
        return null;
    }
    IEnumerator CreateEnemyPull()
    {
        while (!GameManger.Ginstance.isGameOver)
        {
            yield return new WaitForSeconds(3f);
            if (GameManger.Ginstance.isGameOver)
                yield break;
            // 게임이 종료가 되면 코루틴을 종료 해서 다음 루틴을 진행 하지 않ㅇ므

            foreach (GameObject _enemy in EnemyPoolList)
            {           // 활성화하는지 않하는지 판별하는 변수
                if (_enemy.activeSelf == false)
                {
                    int idx = Random.Range(0,SpwanPointsList.Count);
                    _enemy.transform.position = SpwanPointsList[idx].position;
                    _enemy.transform.rotation = SpwanPointsList[idx].rotation;
                    _enemy.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
