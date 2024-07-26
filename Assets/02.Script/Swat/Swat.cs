using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
[RequireComponent(typeof(NavMeshAgent))]
public class Swat : MonoBehaviour
{
    // 패트롤 지점을 담기 위한  List 제네릭(일반형) 변수
    private NavMeshAgent agent;
    public List<Transform> WayPointList; //  순찰 지점의 배열 인덱스 값
    public int NextIdex = 0; //
    private bool _patrolling;
    private readonly float petrolSpeed = 1.5f;
    private readonly float turnSpeed = 1.5f;
    private float damping = 1.0f; //회전할때 속도 조절 계순
    private Transform Enemytr;

    public bool patroiing // 프로 퍼티
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = petrolSpeed;
                damping = 1.0f;
                MoveWayPoint();
            }


        }
    }
    // 
    private Vector3 _traceTaget;
    public Vector3 traceTaget
    {
        // 선언된 _traceTaget을  가져와
        get { return _traceTaget; }
        set
        {
            // 스피드 값과 함수의 값을 할당
            _traceTaget = value;
            agent.speed = petrolSpeed;
            damping = 7.0f;
            TraceTarget(_traceTaget);
        }
    }
    public float speed
    {
        // NevMesh
        get { return agent.velocity.magnitude; }
    }

    void Start()
    {
        Enemytr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        // 에이전트 이용하여 회전하는 기능을 비활성화하는 이유  이후 부드럽지않ㅇ므
        var Group = GameObject.Find("WayPointGroup");
        // 하이라키이에 있는 게임 오브젝트명 이 WayPointGrounp 를 찾아서 대입
        if (Group != null) // 유효성 검사
        {
            // 하위 오브젝트의 트랜스 폼을 wayPoint List에 다 담는다.
            Group.GetComponentsInChildren<Transform>(WayPointList);
            WayPointList.RemoveAt(0); // 첫번째 인덱스는 삭제

        }
        MoveWayPoint();
    }


    void Update()
    {
        // 계속 추적중이라면
        if (agent.isStopped == false)
        {
            // NavMeshAgent 가야 할 방향 벡터를 쿼터니언 타입의 각도로 변환
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //                  보간함수를 이용해서 점진적으로 부드럽게 회전 시킬수 있다.
            Enemytr.rotation = Quaternion.Slerp(Enemytr.rotation, rot, Time.deltaTime * damping);
        }
        if (_patrolling == false) return;

        //float distance = Vector3.Distance(transform.position, WayPointList[NextIdex].position);
        //float distance2 =(WayPointList[NextIdex].position-transform.position).magnitude;
        // 다음 도착지점이 0.5f보다 작다면
        if (agent.remainingDistance <= 0.5f)
        {
            agent.speed = 6f;
            NextIdex = ++NextIdex % WayPointList.Count;
            MoveWayPoint();
        }
    }

    void MoveWayPoint()
    {
        // 최단 경로 계산이 끝나지 않고 길을 잃어버린경우
        if (agent.isPathStale)
        {
            return;
        }
        // 네비메쉬 추적거리가 waypointlist 배열의 위치가 같아야한다.
        agent.destination = WayPointList[NextIdex].position;
        agent.isStopped = false;
    }

    private void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }
    /// <summary>
    ///  멈추는 함수
    /// </summary>
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }
}

