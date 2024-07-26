using System.IO; //  ���� ������� ���� ���ӽ����̽�
using System.Runtime.Serialization.Formatters.Binary;
// ���̳ʸ� ������ ���� ���ӽ����̽� 
// ����ȭ �ڵ��Ҷ� �����͸� float int string�� Byte�� �ٲ�鼭 ��ǻ�Ͱ� ���� �� �ְ� �ٲ۴�.
// �� ����ȭ
// byte�� �ٲ� �ϵ��ũ�� ����� �����͸� ����Ƽ�� �ε� �Ҷ��� ������ȭ �Ͽ� byte�� float int string ���� ��ȯ�ȴ�.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;

public class DataManger : MonoBehaviour
{
    [SerializeField] string dataPath; // ������

    public void Initialize()//�ʱ�ȭ �ϴ� �Լ�
    {
        dataPath = Application.persistentDataPath + "/gameDate.dat";
        // ���� ���� ��ο� ���ϸ� ���� ���� ������ �ڵ� ������ �ȴ�.
        
    }
    public void Save(GameData gameData)
    {
        // ���̳ʸ� ���� ������ ���� BinaryFormatter ����
        BinaryFormatter bf = new BinaryFormatter();
        // ������ ������  ���� ���� ����
        FileStream file = File.Create(dataPath);
        // ���Ͽ� ������ Ŭ������ ������ �Ҵ�
        GameData data = new GameData();
        data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.damage = gameData.damage;
        data.equipItem = gameData.equipItem;
        data.speed = gameData.speed;
        // ����ȭ ������ ��ħ
        bf.Serialize(file, data);
        file.Close();
        // �ȴݰ� ���� ������ ���� �Ѵ�.�޸𸮰� ����� ���� �����ϱ� ������
        // 
    }
    public GameData Load()
    {
        // �������� ��ȿ�� �˻縦 ����
        if (File.Exists(dataPath))
        {

            // ������ ���� �ϴ� ��� �ҷ����� 
            BinaryFormatter bf = new BinaryFormatter();
            // ������ �����ִٸ�
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);    // ������ȭ �Ͽ� �ҷ�����
            file.Close();
            return data;
        }
        // ������ ���°��
        else
        {
            GameData data = new GameData();
            return data;
        }
    }

}
