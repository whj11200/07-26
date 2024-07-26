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
        // ���� ����ü�� �� ���� ����ü�� ���� ��ġ���� ���� 0.02�� �÷� ������ġ ������ ����
        Ray ray = new Ray(firepos.position + (Vector3.up * 0.02f),Tr.forward);
        RaycastHit hit;
        Debug.DrawLine(ray.origin,ray.direction*100f,Color.blue);
  
        if (Input.GetMouseButtonDown(0))
        {
            // ���� �������� ù��° ���� ��ġ ����
                                            //������ǥ ������ ������ǥ �������� ����
            lineRenderer.SetPosition(0, Tr.InverseTransformPoint(ray.origin));
            // ���� ��ü�� ������ ���� ������ ������ ray���̰� �����°� hit== �Ÿ��� 100f
            if (Physics.Raycast(ray,out hit , 100f))
            {
                
                lineRenderer.SetPosition(1, Tr.InverseTransformPoint(hit.point));
            }
            // ���� �ʾ����� ������ 100���� ��´�.
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
