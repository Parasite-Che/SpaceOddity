using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;


public class LocalUserInfo
{
    public int id;
    public string name = "";
    public bool isVip = false;
    public int scores = 0;
    public int money = 0;
    public bool sound_state = true;
    public bool music_state = true;
    public bool notice_state = true;
    public float sound_volume = 1;
    public float music_volume = 1;
    public string control_type = "touch";
    public string language = "";
    public bool isAgreePrivacy = false;
    public int level_id = 0;
    public int shipsReward;
    public int bonusesReward;
    public bool[] tutorComplete = {false, false, false};

}

public class LocalUserStatistics
{
    public int total_coins = 0;
    public int total_planets = 0;
    public int total_death = 0;
    public int total_obstacle = 0;
    public int total_missions = 0;
    public int total_ads = 0;
    
}

public class AchievementControl
{
    public int id;
    public string title = "";
    public string description = "";
    public string local_icon = "";
    public string local_icon_finished = "";
    public string category = "";
    public Award award;
    public Condition condition;
    public bool finished = false;
    public bool awardClaimed;

    [System.NonSerialized] public IAchievementController AchievementController;
    
    public IAchievementController AchievementControllerInitializer()
    {
        if (condition.site != "")
        {
            Debug.Log(condition.site);
            return new SiteAchievementController();
        }
        else if (condition.gameStretch != 0)
        {
            Debug.Log(condition.gameStretch);
            return new GameStretchAchievementController();
        }
        else if (condition.shipsBought != 0)
        {
            Debug.Log(condition.shipsBought);
            return new ShipsBoughtAchievementController();
        }
        else if (condition.galaxy != "")
        {
            Debug.Log(condition.galaxy);
            return new GalaxyAchievementController();
        }
        else if (condition.coins != 0)
        {
            Debug.Log(condition.coins);
            return new MoneyAchievementController();
        }
        else if (condition.level != 0)
        {
            Debug.Log(condition.level);
            return new LevelAchievementController();
        }
        else if (condition.planets != 0)
        {
            Debug.Log(condition.planets);
            return new PlanetAchievementController();
        }
        else if (condition.obstacles != 0)
        {
            Debug.Log(condition.obstacles);
            return new ObstacleAchievementController();
        }
        else if(condition.tutorialFinished != false)
        {
            return new TutorialAchievementController();
        }
        else
        {
            return null;
        }

    }
    
}

public class LocalStoreItem
{
    public int id;
    public string title;
    public string description;
    public string local_image;
}

public class LocalShip
{
    public int id;
    public string title;
    public string filename;
    public string folder;
}

public class LocalPack
{
    public int id;
    public string title;
    public string subtitle;
    public string local_pack;
}

public class GameStretch
{
    public DateTime start;
    public DateTime end;
}

public class UserLocalData
{
    public LocalUserInfo LocalUserInfo;
    public AchievementControl[] AchievementControlList;
    //public List <LocalUserLocalization> LocalUserLocalizations;
    public LocalUserStatistics LocalUserStatistics;
    public List<LocalStoreItem> LocalStoreItemList;
    public List<LocalShip> LocalShipList;
    public List<LocalPack> LocalPackList;
    public List<GameStretch> GameStretchList;

    public void UserLocalDataInitializer()
    {
        LocalUserInfo = new LocalUserInfo();
        //LocalUserLocalizations = new List<LocalUserLocalization>();
        LocalUserStatistics = new LocalUserStatistics();
        LocalStoreItemList = new List<LocalStoreItem>();
        LocalShipList = new List<LocalShip>();
        LocalPackList = new List<LocalPack>();
        GameStretchList = new List<GameStretch>();
    }
    
    public void GetAward(int id)
    {
        if (AchievementControlList[id].award.coins != 0)
        {
            LocalUserStatistics.total_coins += AchievementControlList[id].award.AwardReturn();
            LocalUserInfo.money += AchievementControlList[id].award.AwardReturn();
        }
        else if (AchievementControlList[id].award.ships != 0)
        {
            LocalUserInfo.shipsReward += AchievementControlList[id].award.AwardReturn();
        }
        else if (AchievementControlList[id].award.bonus != 0)
        {
            LocalUserInfo.bonusesReward += AchievementControlList[id].award.AwardReturn();
        }
        JSONTest.UserLocalDataObject.AchievementControlList[id].awardClaimed = true;
    }
    
}
