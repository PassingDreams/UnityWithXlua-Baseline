using UnityEngine;
using System.IO;

/*定义一个数据类
[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int playerScore;
}*/

public static class JsonSaveManager 
{
    
    // 存储数据到文件
    public static void SaveDataTo<T>(T data,string fileName)
    {
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + string.Format("/{0}.json", fileName);
        File.WriteAllText(path, json);
    }
    
    /*public static void SaveDataTo(object data,string fileName)
    {
        string json = JsonConvert.SerializeObject(data); // plastic
        string path = Application.persistentDataPath + string.Format("/{0}.json", fileName);
        File.WriteAllText(path, json);
    }*/

    // 从文件中读取数据
    public static T LoadData<T>(string fileName) where T:class
    {
        string path = Application.persistentDataPath + string.Format("/{0}.json", fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }
        else
        {
            Debug.LogWarning("No data file found:"+path);
            return null;
        }
    }
}
