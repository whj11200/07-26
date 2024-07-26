using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizomos : MonoBehaviour
{
    public enum Type {NORMAL,WAYPOINT}
    // const�� �ٲ����ʰ� �����ϱ�
    private const string wayPointFile = "Enemy";
    public Type type = Type.NORMAL;

    public Color _color;
    public float _radius;
    void Start()
    {

    }


    private void OnDrawGizmos() //�� ȭ�鿡�� �����̳� ���� �׷��ִ� �Լ�
                                // ����Ƽ �����Լ�
    {
       if (type  == Type.NORMAL)
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
       else
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position + Vector3.up , wayPointFile, true);
            // ��ġ                            // ���ϸ� , ������ ���뿩��
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
      
    }
}
