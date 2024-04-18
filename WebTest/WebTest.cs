using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


// если входим с мобильного интернета

public class WebTest : MonoBehaviour
{
    public delegate void OnRequestSuccess(string answer);
    public static OnRequestSuccess onRequestSuccess { get; set; }

    public delegate void OnRequestFailure(UnityWebRequest.Result errorCode);
    public static OnRequestFailure onRequestFailure { get; set; }

    private string deviceID;
    private string authorizationToken;
    private const string uriBase = "https://sp.perimeter.games/api/v1";
    private const string uriRegistration = "/user/create";
    private const string uriToken = "/token";
    private const string uriGlobalAchievements = "/application/getAchievements";
    
    public JSONTest jsonTest;
    public Text debugText;

    public UserData userData;

    public List<string> textureDownloadList;
    public int texturesToLoad;
    public int texturesLoadedSuccess;
    public int texturesLoadedFailure;

    List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
    private bool isUserRegistered = false;
    private bool isConnected = true;
    private bool isUserInitialized = false;

    void Start()
    {
        deviceID = SystemInfo.deviceUniqueIdentifier;
        //StartCoroutine(Initialize());
    }

    public IEnumerator Initialize()
    {
        Debug.Log("Инициализируем пользователя");
        yield return StartCoroutine(InitializeUser());
        Debug.Log("Инициализация пользователя закончена");
        if (isUserRegistered)
        {
            yield return StartCoroutine(LoadGlobalAchievements());
        }
    }

    // пока что используем в качестве имени и пароля id устройства
    public IEnumerator InitializeUser()
    {
        // пытаемся получить токен
        // если не удаётся получить токен, регистрируем пользователя
        paramList.Clear();
        paramList.Add(new KeyValuePair<string, string>("name", deviceID));
        paramList.Add(new KeyValuePair<string, string>("password", deviceID));
        yield return StartCoroutine(SendPostRequest(LoadUserDataSuccess, LoadUserDataFailure, paramList, uriBase + uriToken));

        // подключение отсутствует, нет смысла продолжать
        if (!isConnected)
        {
            yield break;
        }
        // подключение есть, но получить токен не удалось
        // значит имя и пароль неправильные, нужно регистрировать нового юзера
        if (!isUserRegistered)
        {
            paramList.Clear();
            paramList.Add(new KeyValuePair<string, string>("name", deviceID));
            paramList.Add(new KeyValuePair<string, string>("password", deviceID));
            yield return StartCoroutine(SendPostRequest(LoadUserDataSuccess, LoadUserDataFailure, paramList, uriBase + uriRegistration));
            if (isUserRegistered) isUserInitialized = true;
        }
    }

    public IEnumerator LoadGlobalAchievements()
    {
        paramList.Clear();
        paramList.Add(new KeyValuePair<string, string>("locale", "ru"));
        yield return StartCoroutine(SendPostRequest(GetGlobalAchievementsSuccess, GetGlobalAchievementsFailure, 
                                                    authorizationToken, paramList, uriBase + uriGlobalAchievements));
    }
    
    public void LoadUserDataSuccess(string jsonData)
    {
        Debug.Log("Получили данные пользователя. Сохраняем.");
        isUserRegistered = true;

        userData = JsonUtility.FromJson<UserData>(jsonData);
        authorizationToken = userData.token_type + " " + userData.token;
    }

    public void LoadUserDataFailure(UnityWebRequest.Result errorCode)
    {
        Debug.Log("Не удалось получить данные пользователя");
        isUserRegistered = false;
        
        if (errorCode == UnityWebRequest.Result.ConnectionError)
        {
            isConnected = false;
            Events.onConnectionFailure?.Invoke();
        }
    }

    public void GetGlobalAchievementsSuccess(string achievementsJson)
    {
        Debug.Log("Получили данные об ачивках.");
        jsonTest.LoadAchievements(achievementsJson);
    }

    public void GetGlobalAchievementsFailure(UnityWebRequest.Result errorCode)
    {
        Debug.Log("Не удалось получить данные об ачивках");
        if (errorCode == UnityWebRequest.Result.ConnectionError)
        {
            isConnected = false;
            Events.onConnectionFailure?.Invoke();
        }
    }

    public void LoadTextureSuccess(string str)
    {
        ;
    }


    public IEnumerator SendPostRequest(OnRequestSuccess successAction, OnRequestFailure failureAction, string authorizationToken,
                                        List<KeyValuePair<string, string>> paramList, string uri)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> pair in paramList)
        {
            form.AddField(pair.Key, pair.Value);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            Debug.Log("Отправляем реквест");
            www.SetRequestHeader("Authorization", authorizationToken);
            yield return www.SendWebRequest();

            Debug.Log("Отправили реквест");
            Debug.Log(www.downloadHandler.text);

            if (www.result != UnityWebRequest.Result.Success)
            {
                failureAction.Invoke(www.result);
            }
            else
            {
                successAction.Invoke(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator SendPostRequest(OnRequestSuccess successAction, OnRequestFailure failureAction,
                                        List<KeyValuePair<string, string>> paramList, string uri)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> pair in paramList)
        {
            form.AddField(pair.Key, pair.Value);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            Debug.Log("Отправляем реквест");
            yield return www.SendWebRequest();

            Debug.Log("Отправили реквест");
            Debug.Log(www.downloadHandler.text);

            if (www.result != UnityWebRequest.Result.Success)
            {
                failureAction.Invoke(www.result);
            }
            else
            {
                successAction.Invoke(www.downloadHandler.text);
            }
        }
    }

    IEnumerator LoadTexture(OnRequestSuccess successAction, OnRequestFailure failureAction, string uri)
    {
        UnityWebRequest wr = new UnityWebRequest(uri);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;

        yield return wr.SendWebRequest();
        
        if (wr.result == UnityWebRequest.Result.Success)
        {
            Texture2D t = texDl.texture;
            Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height),
                Vector2.zero, t.width);
            Debug.Log(s.pixelsPerUnit);
        }
        else
        {

        }
    }

    IEnumerator DownloadAssetBundle()
    {
        UnityWebRequest www = new UnityWebRequest("https://sp.perimeter.games//galaxy.unitypakage");
        DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
        www.downloadHandler = handler;
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Extracts AssetBundle
            AssetBundle bundle = handler.assetBundle;
            Debug.Log(bundle.GetAllAssetNames());
        }
    }
    
}
