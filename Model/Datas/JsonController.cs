using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public static class JsonController
{
    public static void SavePurchaseInfoJson(PurchaseInfo shipsPurchaseInfo)
    {
        string path = Application.dataPath;
        string mobilePath = Application.persistentDataPath;
        string json = JsonConvert.SerializeObject(shipsPurchaseInfo);

#if UNITY_EDITOR
        File.WriteAllText(path + "/ShipsPurchaseInfo.json", json);
        Debug.Log("Unity Editor");
#elif UNITY_ANDROID
        File.WriteAllText(mobilePath + "/ShipsPurchaseInfo.json", json);
#endif
    }

    public static PurchaseInfo LoadPurchaseInfoJson()
    {
        string path = Application.dataPath;
        string mobilePath = Application.persistentDataPath;
        PurchaseInfo lvl = new PurchaseInfo();
#if UNITY_EDITOR
        lvl =
            JsonConvert.DeserializeObject<PurchaseInfo>
                (File.ReadAllText(path + "/ShipsPurchaseInfo.json"));
#elif UNITY_ANDROID
        lvl =
            JsonConvert.DeserializeObject<PurchaseInfo>
            (File.ReadAllText(mobilePath + "/ShipsPurchaseInfo.json"));
#endif
        return lvl;
    }

    public static PurchaseInfo LoadShipsPurchaseInfoJsonFromResources()
    {
        string path = "Jsons/ShipsPurchaseInfo.json";
        string newpath = path.Replace(".json", "");
        TextAsset ta = Resources.Load<TextAsset>(newpath);
        string json = ta.text;
        PurchaseInfo shipInfo = JsonConvert.DeserializeObject<PurchaseInfo>(json);
        return shipInfo;
    }

    public static Localization LoadLocalizationfromRes(string languageName)
    {
#if UNITY_EDITOR
        string path = Application.dataPath + "/languages/" + languageName + ".json";
#elif UNITY_ANDROID
        string path = Application.persistentDataPath + "/languages/" + languageName + ".json";
#endif
        
        if (File.Exists(path))
        {
            Localization localization = JsonConvert.DeserializeObject<Localization>(
                File.ReadAllText(path));
            return localization;
        }
        else
        {
            Debug.LogError("Файла локализации (" + path + ") не существует");
            return null;
        }
    }
    
    public static UserLocalData LoadUserDataFromRes()
    {
#if UNITY_EDITOR
        string path = Application.dataPath + "/LocalData/UserLocalData.json";
#elif UNITY_ANDROID
        string path = Application.persistentDataPath + "/LocalData/UserLocalData.json";
#endif
        
        if (File.Exists(path))
        {
            UserLocalData localData = JsonConvert.DeserializeObject<UserLocalData>(File.ReadAllText(path));
            Debug.LogWarning(File.ReadAllText(path));
            return localData;
        }
        else
        {
            Debug.LogError("Файла с данными (" + path + ") не существует");
            return null;
        }
    }

    public static void SaveJsonOnDevice(string json, string fileName, string devicePath)
    {
        if (Directory.Exists(devicePath) == false) {
            Directory.CreateDirectory(devicePath);
        }
        File.WriteAllText(devicePath + fileName + ".json", json);
    }
}


[JsonObject]
public class Localization
{
    public Hashtable localText = new Hashtable();
}

[JsonObject]
public class PurchaseInfo
{
    public Hashtable ShipPurchaseInfo = new Hashtable(); // ключ: имя корабля, значение: y - куплен, n - не куплен

}

[JsonObject]
public class ProfileInformation
{
    public string Nickname;
    public int Coins;
    public float Score;
    
}
