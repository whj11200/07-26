using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLaser : MonoBehaviour
{
    private Transform Tr;
    private LineRenderer lineRenderer;
    [SerializeField]    
    private Transform firepos;
    [SerializeField]
    private Player fireCtrl;
    void Start()
    {
        Tr = transform;

        firepos = transform.GetComponentsInParent<Transform>()[1];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.enabled = false;
        fireCtrl = GetComponentInParent<Player>();
    }

  
    void Update()
    {
        if (Time.timeScale <= 0f) return;
            if (EventSystem.current.IsPointerOverGameObject())
        
            return;
        
        if (fireCtrl.is_reload)
            return;
        // 레이 구조체를 새 레이 구조체에 현재 위치에서 위로 0.02씩 늘려 현재위치 앞으로 생성
        Ray ray = new Ray(firepos.position + (Vector3.up * 0.02f),Tr.forward);
        RaycastHit hit;
        Debug.DrawLine(ray.origin,ray.direction*100f,Color.blue);
  
        if (Input.GetMouseButtonDown(0))
        {
            // 라인 렌더러의 첫번째 점의 위치 설정
                                            //월드좌표 방향을 로컬좌표 방향으로 변경
            lineRenderer.SetPosition(0, Tr.InverseTransformPoint(ray.origin));
            // 만약 물체에 광선이 맞을 았을시 방향은 ray쪽이고 나오는건 hit== 거리는 100f
            if (Physics.Raycast(ray,out hit , 100f))
            {
                
                lineRenderer.SetPosition(1, Tr.InverseTransformPoint(hit.point));
            }
            // 맞지 않았을때 끝점을 100으로 잡는다.
            else
            {
                lineRenderer.SetPosition(1, Tr.InverseTransformPoint(ray.GetPoint(100f)));

            }
            StartCoroutine(ShowRaser());

        }

    }
    IEnumerator ShowRaser()
    {
        lineRenderer.enabled=true;
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
    }
}
