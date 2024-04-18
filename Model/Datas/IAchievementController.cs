using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAchievementController
{
    public int CheckAndReturnInt(int i);
    
    public string CheckAndReturnStr(int i);

}

public class MoneyAchievementController : IAchievementController
{
    public int CheckAndReturnInt(int i)
    {
        if (JSONTest.UserLocalDataObject.LocalUserStatistics.total_coins >=
            JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt())
        {
            JSONTest.UserLocalDataObject.AchievementControlList[i].finished = true;
            //JSONTest.UserLocalDataObject.GetAward(i);
        }
        Debug.LogWarning(JSONTest.UserLocalDataObject.LocalUserStatistics.total_coins + " - total_coins : condition - " + 
                  JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt());
        return JSONTest.UserLocalDataObject.LocalUserStatistics.total_coins;
    }

    public string CheckAndReturnStr(int i)
    {
        return "";   
    }

}

public class LevelAchievementController : IAchievementController
{
    public int CheckAndReturnInt(int i)
    {
        if (JSONTest.UserLocalDataObject.LocalUserInfo.level_id >= 
            JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt())
        {
            JSONTest.UserLocalDataObject.AchievementControlList[i].finished = true;
            //JSONTest.UserLocalDataObject.GetAward(i);
        }
        Debug.LogWarning(JSONTest.UserLocalDataObject.LocalUserInfo.level_id + " - level_id : condition - " + 
                         JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt());
        return JSONTest.UserLocalDataObject.LocalUserInfo.level_id;
    }
    
    public string CheckAndReturnStr(int i)
    {
        return "";   
    }
    
}

public class ObstacleAchievementController : IAchievementController
{
    public int CheckAndReturnInt(int i)
    {
        if (JSONTest.UserLocalDataObject.LocalUserStatistics.total_obstacle >= 
            JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt())
        {
            JSONTest.UserLocalDataObject.AchievementControlList[i].finished = true;
            //JSONTest.UserLocalDataObject.GetAward(i);
        }
        Debug.LogWarning(JSONTest.UserLocalDataObject.LocalUserStatistics.total_obstacle + " - total_obstacle : condition - " + 
                         JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt());
        return JSONTest.UserLocalDataObject.LocalUserStatistics.total_obstacle;
    }
    
    public string CheckAndReturnStr(int i)
    {
        return "";   
    }
    
}

public class PlanetAchievementController : IAchievementController
{
    public int CheckAndReturnInt(int i)
    {
        if (JSONTest.UserLocalDataObject.LocalUserStatistics.total_planets >= 
            JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt())
        {
            JSONTest.UserLocalDataObject.AchievementControlList[i].finished = true;
            //JSONTest.UserLocalDataObject.GetAward(i);
        }
        Debug.LogWarning(JSONTest.UserLocalDataObject.LocalUserStatistics.total_planets + " - total_planets : condition - " + 
                         JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt());
        return JSONTest.UserLocalDataObject.LocalUserStatistics.total_planets;
    }
    
    public string CheckAndReturnStr(int i)
    {
        return "";   
    }
    
}

public class SiteAchievementController : IAchievementController
{
    public int CheckAndReturnInt(int i)
    {
        return -1;
    }
    
    public string CheckAndReturnStr(int i)
    {
        return "";   
    }
}

public class GameStretchAchievementController : IAchievementController
{
    public int CheckAndReturnInt(int i)
    {
        if (JSONTest.UserLocalDataObject.GameStretchList.Count >=
            JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt())
        {
            JSONTest.UserLocalDataObject.AchievementControlList[i].finished = true;
            //JSONTest.UserLocalDataObject.GetAward(i);
        }
        Debug.LogWarning(JSONTest.UserLocalDataObject.GameStretchList.Count + " - GameStretchList.Count : condition - " + 
                         JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt());

        return JSONTest.UserLocalDataObject.GameStretchList.Count;
    }
    
    public string CheckAndReturnStr(int i)
    {
        return "";   
    }
}

public class ShipsBoughtAchievementController : IAchievementController
{
    public int CheckAndReturnInt(int i)
    {
        if (JSONTest.UserLocalDataObject.LocalShipList.Count >= 
            JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt())
        {
            JSONTest.UserLocalDataObject.AchievementControlList[i].finished = true;
        }
        Debug.LogWarning(JSONTest.UserLocalDataObject.LocalShipList.Count + " - LocalShipList.Count : condition - " + 
                         JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnInt());
        return JSONTest.UserLocalDataObject.LocalShipList.Count;
    }
    
    public string CheckAndReturnStr(int i)
    {
        
        return "";   
    }
    
}

public class GalaxyAchievementController : IAchievementController
{
    public int CheckAndReturnInt(int i)
    {
        return -1;
    }
    
    public string CheckAndReturnStr(int i)
    {
        return JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnString();   
    }
    
}

public class TutorialAchievementController : IAchievementController
{
    public int CheckAndReturnInt(int i)
    {
        for (int j = 0; j < 3; j++)
        {
            if (JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[j] == false)
            {
                return -1;
            }
        }
        
        return 1;
    }
    
    public string CheckAndReturnStr(int i)
    {
        return JSONTest.UserLocalDataObject.AchievementControlList[i].condition.ConditionReturnString();   
    }
    
}

