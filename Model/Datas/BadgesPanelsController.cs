using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BadgesPanelsController : MonoBehaviour
{
    public static IEnumerator CreateBadgesPanelsInList(GameObject content, GameObject badgesPrefab, 
        BadgesPanelInfo[] badgesPanelInfos, TMP_Text loadingText, Slider bar)
    {
        for (int i = 0; i < badgesPanelInfos.Length; i++)
        {
            GameObject panel = Instantiate(badgesPrefab, content.transform);
            panel.transform.GetChild(0).GetComponent<Image>().sprite = 
                FromResToObj.SpriteFromRes(badgesPanelInfos[i].ImageSource /*"/sprites/achievements_icons/thumb1.png"*/, 
                    500, 500);
            panel.transform.GetChild(1).GetComponent<TMP_Text>().text = badgesPanelInfos[i].Title;
            panel.transform.GetChild(2).GetComponent<TMP_Text>().text = badgesPanelInfos[i].Desscription;
            bar.value = ((float)(i + 1) / badgesPanelInfos.Length);
            loadingText.text = "Расставление достижений по местам: " + (i + 1).ToString() + "/" + badgesPanelInfos.Length;
            yield return null;
        }
    }
}

public struct BadgesPanelInfo
{
    public string ImageSource;
    public string Title;
    public string Desscription;
}