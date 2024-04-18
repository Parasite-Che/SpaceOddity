using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UserLocalizationJSON
{
    public int id;
}

[System.Serializable]
public class UserStatisticsJSON
{
    public string guid;
}

[System.Serializable]
public class UserDataEntry
{
    public int id;
    public string name;
    public UserStatisticsJSON statistics;
    public UserLocalizationJSON localization;
}

[System.Serializable]
public class UserData
{
    public string token;
    public string token_type;
    public UserDataEntry data;
}
