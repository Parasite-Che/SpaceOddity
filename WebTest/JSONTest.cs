using System;
using System.Collections;
using System.Collections.Generic;
//using IronSourceJSON;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.IO;
using Newtonsoft.Json;


[System.Serializable]
public class AchievementDTO
{
    public int id;
    public string title;
    public string description;
    public string icon;
    public string icon_finished;
    public string condition;
    public string award;
    public string category;
    public string created_at;
    public string updated_at;
}

public class Award
{
    public int coins = 0;
    public int ships = 0;
    public int bonus = 0;

    public int AwardReturn()
    {
        if (coins != 0)
        {
            return coins;
        }
        else if (ships != 0)
        {
            return ships;
        }
        else if (bonus != 0)
        {
            return bonus;
        }
        else
        {
            Debug.LogError("Не указана награда");
            return -1;
        }
    }

    public string AwardTypeReturn()
    {
        if (coins != 0)
        {
            return "coins";
        }
        else if (ships != 0)
        {
            return "ships";
        }
        else if (bonus != 0)
        {
            return "bonus";
        }
        else
        {
            Debug.LogError("Не указана награда");
            return "";
        }
    }
}

public class Condition
{
    public string site = "";
    public int gameStretch = 0;
    public bool tutorialFinished = false;
    public int shipsBought = 0;
    public bool allShips = false;
    public string galaxy = "";
    public int coins = 0;
    public int level = 0;
    public int planets = 0;
    public int obstacles = 0;

    public int ConditionReturnInt()
    {
        if (gameStretch != 0)
        {
            return gameStretch;
        }
        else if (shipsBought != 0)
        {
            return shipsBought;
        }
        else if (coins != 0)
        {
            return coins;
        }
        else if (level != 0)
        {
            return level;
        }
        else if (planets != 0)
        {
            return planets;
        }
        else if (obstacles != 0)
        {
            return obstacles;
        }
        else
        {
            Debug.LogError("Не указано условие int");
            return -1;
        }
    }

    public string ConditionReturnString()
    {
        if (site != "")
        {
            return site;
        }
        else if (galaxy != "")
        {
            return galaxy;
        }
        else
        {
            Debug.LogError("Не указано условие string");
            return "ERROR";
        }
    }

    public bool ConditionReturnBool()
    {
        if (tutorialFinished)
        {
            return true;
        }
        else if (allShips)
        {
            return true;
        }
        else
        {
            Debug.LogError("Не указано условие bool");
            return false;
        }

    }


}

[System.Serializable]
public class AchievementDTOList
{
    public AchievementDTO[] data;
}

public class JSONTest : MonoBehaviour
{
    public AchievementDTOList achievementDTOList = new AchievementDTOList();

    public static UserLocalData UserLocalDataObject;
    
    private BadgesMenu badgesMenu;
    private string loadDataMethod = "";

    private void Awake()
    {
        badgesMenu = new BadgesMenu();
    }

    public void LoadAchievements(string json)
    {
        achievementDTOList = JsonUtility.FromJson<AchievementDTOList>(json);
    }

    public IEnumerator UserLocalDataInitializer()
    {
#if UNITY_EDITOR
        string path = Application.dataPath;
#elif UNITY_ANDROID
        string path = Application.persistentDataPath;
#endif
        UserLocalDataObject = new UserLocalData();
        UserLocalDataObject.UserLocalDataInitializer();
        //yield return StartCoroutine(InitializeUserLocalDataFromServer());
        if (File.Exists(path + "/LocalData/UserLocalData.json") == false)
        {
            yield return StartCoroutine(AchievementControlInitializer());
            SaveUserLocalDataOnDevice();
        }
        else {
            yield return StartCoroutine(InitializeUserLocalDataFromResources());
            //AllAchievementCheck();
        }
        
        //yield return StartCoroutine(AchievementControlInitializer());
        
        yield return null; 
    }

    private IEnumerator InitializeUserLocalDataFromServer()
    {
        yield return StartCoroutine(AchievementControlInitializer());
        
        //  Тестовая инициализация
        UserLocalDataObject.LocalUserInfo = new LocalUserInfo();
        UserLocalDataObject.LocalUserStatistics = new LocalUserStatistics();
    }

    private IEnumerator InitializeUserLocalDataFromResources()
    {
        UserLocalDataObject = JsonController.LoadUserDataFromRes();
        //Debug.LogWarning(UserLocalDataObject);
        yield return null;
    }


    private IEnumerator AchievementControlInitializer()
    {
#if UNITY_EDITOR
        string path = Application.dataPath;
#elif UNITY_ANDROID
        string path = Application.persistentDataPath;
#endif
        UserLocalDataObject.AchievementControlList = new AchievementControl[achievementDTOList.data.Length];
        for (int i = 0; i < UserLocalDataObject.AchievementControlList.Length; i++)
        {
            UserLocalDataObject.AchievementControlList[i] = new AchievementControl();
            UserLocalDataObject.AchievementControlList[i].id = achievementDTOList.data[i].id;
            UserLocalDataObject.AchievementControlList[i].title = achievementDTOList.data[i].title;
            UserLocalDataObject.AchievementControlList[i].description = achievementDTOList.data[i].description;
            UserLocalDataObject.AchievementControlList[i].local_icon = path + "/sprites/achievements_icons/" +
                                                                       achievementDTOList.data[i].id + ".png";
            UserLocalDataObject.AchievementControlList[i].local_icon_finished = "";
            UserLocalDataObject.AchievementControlList[i].finished = false;
            UserLocalDataObject.AchievementControlList[i].awardClaimed = false;
            UserLocalDataObject.AchievementControlList[i].award =
                JsonConvert.DeserializeObject<Award>(achievementDTOList.data[i].award);
            UserLocalDataObject.AchievementControlList[i].condition =
                JsonConvert.DeserializeObject<Condition>(achievementDTOList.data[i].condition);
            UserLocalDataObject.AchievementControlList[i].AchievementController = UserLocalDataObject
                .AchievementControlList[i].AchievementControllerInitializer();
            yield return null;
        }
        
        
    }

    public static void AllAchievementCheck()
    {
        if (UserLocalDataObject != null)
        {
            bool achievementComplete = false;
            //Debug.LogError(UserLocalDataObject.AchievementControlList);
            for (int i = 0; i < UserLocalDataObject.AchievementControlList.Length; i++)
            {
                if (UserLocalDataObject.AchievementControlList[i] == null)
                {
                    //Debug.LogError("UserLocalDataObject.AchievementControlList[i] == null");
                    return;
                }

                if (achievementComplete == false && UserLocalDataObject.AchievementControlList[i].finished == true &&
                    UserLocalDataObject.AchievementControlList[i].awardClaimed == false)
                {
                    achievementComplete = true;
                    Objects.instance.BadgesNotice.SetActive(true);
                }

                if (UserLocalDataObject.AchievementControlList[i].finished == false)
                {
                    //Debug.Log(UserLocalDataObject.AchievementControlList[i].AchievementController);

                    if (UserLocalDataObject.AchievementControlList[i].AchievementController != null)
                    {
                        UserLocalDataObject.AchievementControlList[i].AchievementController.CheckAndReturnInt(i);
                    }
                }
            }
            
            if (achievementComplete == false)
            {
                Objects.instance.BadgesNotice.SetActive(false);
            }
        }
        else
        {
            //Debug.LogError("UserLocalDataObject == null");
        }
    }
    
    public void ChooseLocalData()
    {
        loadDataMethod = "local";
    }
    
    public void ChooseServerData()
    {
        loadDataMethod = "server";
    }

    public static void SaveUserLocalDataOnDevice()
    {
#if UNITY_EDITOR
        string path = Application.dataPath + "/LocalData/";
#elif UNITY_ANDROID
        string path = Application.persistentDataPath + "/LocalData/";
#endif
        JsonController.SaveJsonOnDevice(JsonConvert.SerializeObject(UserLocalDataObject), "UserLocalData", path);
        //Debug.LogError("SAVED");
    }
}
