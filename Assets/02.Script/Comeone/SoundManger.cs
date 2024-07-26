using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    public static SoundManger S_Instance;
    public float SoundVoumn = 1.0f; // 사운드 볼륨
    public bool isSoundMute = false; // 음소거
    public Transform tr;

    void Awake()
    {
        if(S_Instance == null)
        {
            S_Instance = this;

        }
        else if (S_Instance != this)
        {
            Destroy(S_Instance);
        } 
        DontDestroyOnLoad(gameObject);
    }
    public void PlaySound(Vector3 pos, AudioClip cilp)
    {
        if (isSoundMute)
        {
            return; // 음소거 일때 이 함수를 빠져나간다.
        }

        GameObject soundObject = new GameObject("Sound!");
        // 오브젝트 동적 생성
        soundObject.transform.position = pos; // 소리는 위치를 전달 받음
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        // 오디오소스 가 없어 AddComponent<AudioSource>로 오디오 소스를 넣는다.
        
        audioSource.clip = cilp;
        audioSource.minDistance = 20f;
        audioSource.maxDistance = 50f;
        audioSource.volume = SoundVoumn;
        audioSource.Play();
        Destroy(soundObject,cilp.length);
    }

    
    void Update()
    {
        
    }
}
