using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerSek : MonoBehaviour
{
    public static CamerSek instance;
    // 충돌하는 대상
    private Vector3 posCam;
    private Quaternion rotCam;
    private float shackTime;
    public bool isShake = false;

   
    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (isShake)
        {
            float x = Random.Range(-0.1f, 0.1f);
            float y = Random.Range(-0.1f, 0.1f);
            Camera.main.transform.position += new Vector3(x, y, 0f);
            Vector3 ShakeRot =
                new Vector3(0f, 0f, Random.Range(-0.0001f * Time.deltaTime, 0.001f * Time.deltaTime));
            Camera.main.transform.rotation = Quaternion.Euler(ShakeRot);
            if (Time.time - shackTime >= 0.5f)
            {
                isShake = false;
                Camera.main.transform.position = posCam;
                Camera.main.transform.rotation = rotCam;
                //shakeTime = Time.time;
            }
        }

    }
    public void TurnOn()
    {
        isShake = true;
        shackTime = Time.time;
        posCam = Camera.main.transform.position;
        rotCam = Camera.main.transform.rotation;
    }
}


