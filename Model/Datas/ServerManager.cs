using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;


public class ServerManager : MonoBehaviour
{

    #region LoadBundleFromServer(string url, Action<AssetBundle> actionBundle)
    public IEnumerator LoadBundleFromServer(string url, Action<AssetBundle> actionBundle)
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.ProtocolError && request.result != UnityWebRequest.Result.ConnectionError)
        {
            //actionBundle(DownloadHandlerAssetBundle.GetContent(request));
            //bundle = DownloadHandlerAssetBundle.GetContent(request);
            Debug.Log("LOADEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEED");
        }
        else {
            Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
            Debug.Log("NOT LOADEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEED");
        }

        request.Dispose();
    }
    #endregion

    public static bool ExistingCheck(string pathEnd)
    {
#if UNITY_EDITOR
        string path = Application.dataPath;
#elif UNITY_ANDROID
        string path = Application.persistentDataPath;
#endif
        if (File.Exists(path + pathEnd))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    #region LoadJsonFromServer(string url)

    public static IEnumerator LoadLocalizationFromServer(string endUrl, string fileName) //, TMP_Text loadingText, Slider bar)
    {
#if UNITY_EDITOR
        string path = Application.dataPath;
#elif UNITY_ANDROID
        string path = Application.persistentDataPath;
#endif
        Debug.Log("------LoadJsonFromServer------");
        string staticServerUrl = "https://sp.perimeter.games";

        WWW www = new WWW(staticServerUrl + endUrl);
        Debug.Log(staticServerUrl + endUrl);
        //yield return www;
        while (www.isDone != true)
        {
            //bar.value = Math.Abs(www.progress);
           Debug.Log(www.progress);
            //loadingText.text = "Скачивание файла локализации " + fileName + ": " + (www.progress * 100).ToString();
            yield return www;
            //Debug.Log(wwwRequest.isDone);
        }

        if (www.error == null)
        {
            if (File.Exists(path + "/languages/" + fileName + ".json") == false)
            {
                Debug.Log("WWW Success: " + www.text);
                JsonController.SaveJsonOnDevice(www.text, fileName, path + "/languages/");
                //JSONTest.UserLocalDataObject.LocalUserInfo.language = fileName;
            }
            else if (JsonConvert.DeserializeObject<Localization>(www.text) != JsonController.LoadLocalizationfromRes(fileName))
            {
                Debug.Log("WWW overwriting success: " + www.text);
                JsonController.SaveJsonOnDevice(www.text, fileName, path + "/languages/");
            }
            else
            {
                Debug.Log("Файл уже существует");
            }
        }
        else
        {
            Debug.Log("WWW error: " + www.error);
        }

    }

    #endregion
    
    #region LoadImageFromServer(string url)
    public static IEnumerator LoadFileFromServer(string endUrl, string fileName, string directory) //TMP_Text loadingText, Slider bar)
    {
        string staticServerUrl = "https://sp.perimeter.games";
        // ссылка откуда качаем
        var wwwRequest = new UnityWebRequest(staticServerUrl + endUrl);
        wwwRequest.method = UnityWebRequest.kHttpVerbGET;
        // тут куда качаем наш файл в системе, обязательно использовать Application.persistentDataPath
#if UNITY_EDITOR
        string path = Application.dataPath;
#elif UNITY_ANDROID
        string path = Application.persistentDataPath;
#endif
        //Debug.Log(path);
        if (Directory.Exists(path + directory) == false) {
            Directory.CreateDirectory(path  + directory);
        }

        if (File.Exists(path + directory + fileName) == false)
        {
            var dh = new DownloadHandlerFile(path + directory + fileName);
            Debug.Log("------LoadFileFromServer------");
            dh.removeFileOnAbort = true; // Удалить файл при неудачном скачивании
            wwwRequest.downloadHandler = dh;
            
            wwwRequest.SendWebRequest();
            //yield return wwwRequest.SendWebRequest();
            while (wwwRequest.isDone != true)
            {
                //bar.value = Math.Abs(wwwRequest.downloadProgress);
                //loadingText.text = "Скачивание файла " + fileName + ": " + (wwwRequest.downloadProgress * 100).ToString();
                yield return null;
                //Debug.Log(wwwRequest.isDone);
            }
            
            if (wwwRequest.isNetworkError || wwwRequest.isHttpError)
            {
                Debug.Log(wwwRequest.error);
            }
            else
            {
                Debug.Log(wwwRequest.result);
            }
        }
        yield return wwwRequest;
    }
    #endregion

    public static IEnumerator LoadBadgesInPath(AchievementDTOList achievementDtoList, TMP_Text loadingText, Slider bar)
    {
        for (int i = 0; i < achievementDtoList.data.Length; i++)
        {
            yield return LoadFileFromServer(achievementDtoList.data[i].icon, 
                achievementDtoList.data[i].id + ".png", "/sprites/achievements_icons/");
            //Debug.Log(achievementDtoList.data[i].id + ".png");
            
            bar.value = (float)i / achievementDtoList.data.Length;
            loadingText.text = "Badges Checking and loading: " + i + " / " + achievementDtoList.data.Length;
        }
    }
    
}
