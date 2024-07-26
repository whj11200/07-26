using System.IO; //  파일 입출력을 위한 네임스페이스
using System.Runtime.Serialization.Formatters.Binary;
// 바이너리 포맷을 위한 네임스페이스 
// 직렬화 코딩할때 데이터를 float int string을 Byte로 바뀌면서 컴퓨터가 읽을 수 있게 바꾼다.
// 역 직렬화
// byte로 바뀐 하드디스크에 저장된 데이터를 유니티로 로드 할때는 역질렬화 하여 byte가 float int string 으로 변환된다.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;

public class DataManger : MonoBehaviour
{
    [SerializeField] string dataPath; // 저장경로

    public void Initialize()//초기화 하는 함수
    {
        dataPath = Application.persistentDataPath + "/gameDate.dat";
        // 파일 저장 경로와 파일명 지정 공용 폴더에 자동 저장이 된다.
        
    }
    public void Save(GameData gameData)
    {
        // 바이너리 파일 포맷을 위한 BinaryFormatter 생성
        BinaryFormatter bf = new BinaryFormatter();
        // 데이터 저장을  위한 파일 생성
        FileStream file = File.Create(dataPath);
        // 파일에 저장할 클래스에 데이터 할당
        GameData data = new GameData();
        data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.damage = gameData.damage;
        data.equipItem = gameData.equipItem;
        data.speed = gameData.speed;
        // 직렬화 과정을 걸침
        bf.Serialize(file, data);
        file.Close();
        // 안닫고 다음 로직을 진행 한다.메모리가 상당히 많이 차지하기 때문에
        // 
    }
    public GameData Load()
    {
        // 존재유무 유효성 검사를 진행
        if (File.Exists(dataPath))
        {

            // 파일이 존재 하는 경우 불러오기 
            BinaryFormatter bf = new BinaryFormatter();
            // 파일이 열려있다면
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);    // 역직렬화 하여 불러오기
            file.Close();
            return data;
        }
        // 파일이 없는경우
        else
        {
            GameData data = new GameData();
            return data;
        }
    }

}
