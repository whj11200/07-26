using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform taget; // 따라 다닐 대상
    [SerializeField]
    private float Height = 5.0f; // 카메라 높이
    [SerializeField]
    private float distance = 7.0f; //타겟
    [SerializeField]
    private float movedamping = 10f; // 카메라 이동 회전
    [SerializeField]
    private float rotdamping = 15f; //  카메라 회전 이동
    // 카메라가 이동 회전시 떨림을 부드럽게 완화 하는 값
    [SerializeField]
    private Transform CamTr;// 자신 위치
    [SerializeField]
    private float tagetOffset = 2.0f; // 타겟 카메라 높이 값
    void Start()
    {
        CamTr = transform;
        taget = GameObject.FindWithTag("Player").transform;
    }

 
    // Update -> 먼저 된 다음 -> LateUpdate 로 따라간다.
    // FixedUpdate 정확한 시간 타이밍과 정한 물리력을 구현하는 함수
    // FIxedUpdate => Update => LateUpdate 
    void LateUpdate()
    {
        if (taget == null)
            return;// -를 한 이유는 카메라가 플레이어 뒤에 있기때문이다
                   // 타겟 포지션에서 dlstance만큼 뒤에 위치 + Height 높이 만큼 위에 위치
        var Campos = taget.position - (taget.forward * distance)+ (taget.up * Height);
        CamTr.position = Vector3.Slerp(CamTr.position, Campos , Time.deltaTime * movedamping);
                //곡면 보관 (자기자신 위치에서 , Campos 까지 , damping 시간 만큼 부드럽게 움직인다.
        CamTr.rotation = Quaternion.Slerp(CamTr.rotation,taget.rotation, Time.deltaTime * movedamping);
        // 회전값에서 CamTr위치에서 타겟 회전값
        CamTr.LookAt(taget.position +(taget.up * tagetOffset));
                   // 타겟 포지션에서 2만큼 위로 올림

    }
    // 씬화면에서 색상이나 선을 그려주는 함수 콜벡함수
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(taget.position + (taget.up * tagetOffset), 0.1f);
    //                       // 타겟 위치 에 타켓의 위와 위치값 과 Radius
    //   Gizmos.DrawLine(taget.position +(taget.up * tagetOffset),CamTr.position);
    //                 // 타겟 위치값  에 위치 방향와 타겟의 벡터값 그리고 카메라 위치
    //}
}
