using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationController : MonoBehaviour
{
    
    
    public IEnumerator LocalizeText(TMP_Text[] objects, Localization local, Slider Bar, TMP_Text loadingText)
    {
        if (objects != null && local != null)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (local.localText.Contains(objects[i].name))
                {
                    objects[i].text =
                        local.localText[objects[i].gameObject.name].ToString();
                }
                else
                {
                    Debug.LogError("Ключ " + objects[i].name + " не подошёл :(\n"
                                   + "Либо объект неправильно назван, либо ключа ещё не существует");
                }

                if (Bar != null && loadingText != null)
                {
                    Bar.value = (float)(i + 1) / objects.Length;
                    loadingText.text = "Localization process: " + (i + 1) + " / " + objects.Length;
                }
                
                yield return null;
            }
        }
        else
        {
            Debug.LogError("Ошибка локализации (objects == null или local == null)");
        }
    }

    public TMP_Text[] FindTheObjects(string tag)
    {
        TMP_Text[] objList = Resources.FindObjectsOfTypeAll<TMP_Text>().Where(t => t.gameObject.tag == tag).ToArray();
        return objList;
    }
    
    public IEnumerator LocalizationInitializer(GameObject selectingLanguageToDownloadPanel)
    {
        if (JSONTest.UserLocalDataObject.LocalUserInfo.language == "")
        {
            selectingLanguageToDownloadPanel.SetActive(true);
            do
            {
                yield return new WaitForSeconds(.5f);
                Debug.LogWarning("JSONTest.UserLocalDataObject.LocalUserInfo.language == \"\"");
                
            } while (JSONTest.UserLocalDataObject.LocalUserInfo.language == "");

            yield return StartCoroutine(ServerManager.LoadLocalizationFromServer(
                "/storage/localization/" + JSONTest.UserLocalDataObject.LocalUserInfo.language + ".json",
                JSONTest.UserLocalDataObject.LocalUserInfo.language));
            
            selectingLanguageToDownloadPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("User Local Data has localization");
            yield return StartCoroutine(ServerManager.LoadLocalizationFromServer(
                "/storage/localization/" + JSONTest.UserLocalDataObject.LocalUserInfo.language + ".json",
                JSONTest.UserLocalDataObject.LocalUserInfo.language));
        }
    }
}