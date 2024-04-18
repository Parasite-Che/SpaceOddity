using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BadgesMenu : MonoBehaviour
{
    public static BadgesMenu instance;

    private void Awake()
    {
        instance = this;
    }

    private List<Transform> badgeEntryTransformList_;

    public IEnumerator FillBadgesMenu(AchievementDTOList achievementList, TMP_Text loadingText, Slider bar)
    {
        badgeEntryTransformList_ = new List<Transform>();
        int i = 0;
        foreach (AchievementDTO achievement in achievementList.data)
        {
            if (achievement.category == "gameplay")
            {
                CreateAchievementEntryTransform(achievement, Objects.instance.entryGameplayBadgesContainer_, badgeEntryTransformList_);
                
            }
            else if(achievement.category == "social")
            {
                CreateAchievementEntryTransform(achievement, Objects.instance.entrySocialBadgesBContainer_, badgeEntryTransformList_);
            }
            i++; 
            bar.value = (float)i / achievementList.data.Length;
            loadingText.text = "Filling badges menu: " + i + " / " + achievementList.data.Length;
            yield return null;
        }

        //yield return AllBadgesCompletingCheck();
    }

    public void CreateAchievementEntryTransform(AchievementDTO achievement, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(Objects.instance.badgePrefab, container);
        entryTransform.gameObject.SetActive(true);
        //entryTransform.GetComponent<Button>().onClick.AddListener(BadgesMenuOpen);
        entryTransform.GetComponent<BadgeEntry>()?.SetEntryValues(achievement);
        entryTransform.name = entryTransform.GetComponent<BadgeEntry>().achievement.id.ToString();
        
        BadgeCompletingCheck(entryTransform);
        
        transformList.Add(entryTransform);
    }

    public void AllBadgesCompletingCheck()
    {
        JSONTest.AllAchievementCheck();
        AllBadgesInContentCompletingCheck(Objects.instance.entryGameplayBadgesContainer_);
        AllBadgesInContentCompletingCheck(Objects.instance.entrySocialBadgesBContainer_);
    }
    
    private void AllBadgesInContentCompletingCheck(Transform content)
    {
        foreach (Transform badge in content)
        {
            BadgeCompletingCheck(badge);
        }
    }

    private void BadgeCompletingCheck(Transform badge)
    {
        if (JSONTest.UserLocalDataObject.AchievementControlList[badge.GetComponent<BadgeEntry>().achievement.id - 1].finished == false)
        {
            badge.GetChild(0).GetComponent<Image>().color = new Color(0.1568628f,0.1568628f,0.1568628f,0.7450981f);
        }
        else
        {
            badge.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
        }

    }

}
