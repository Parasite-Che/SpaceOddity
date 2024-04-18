using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SceneTextController : MonoBehaviour
{
    public static void TextListUpdater(TMP_Text[] textObjects, string text)
    {
        for (int i = 0; i <textObjects.Length; i++)
        {
            textObjects[i].text = text;
        }
    }

    public static IEnumerator TextCounterInitializer()
    {
        TextListUpdater(Objects.instance.coinsText, JSONTest.UserLocalDataObject.LocalUserInfo.money.ToString());
        TextListUpdater(Objects.instance.scoreText, JSONTest.UserLocalDataObject.LocalUserInfo.scores.ToString());
        TextListUpdater(Objects.instance.sessionScoreText, "0");
        TextListUpdater(Objects.instance.sessionCoinsText, "0");
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("money", 0);

        yield return null;
    } 
}
