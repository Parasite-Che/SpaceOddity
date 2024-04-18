using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BadgeEntry : MonoBehaviour
{
    [SerializeField] public AchievementDTO achievement;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Image icon;
    [SerializeField] private Image icon_finished;
    [SerializeField] private TMP_Text condition;
    [SerializeField] private TMP_Text awardText;
    

    public void SetEntryValues(AchievementDTO achievement_)
    {
        achievement = achievement_;
        title.text = achievement.title;
        description.text = achievement.description;
        condition.text = achievement.condition;
        ConvertAwards();
        icon.sprite = FromResToObj.SpriteFromRes("/sprites/achievements_icons/" + achievement.id + ".png", 
            1500, 1500);
    }

    public void ConvertAwards()
    {
        Award award = JsonUtility.FromJson<Award>(achievement.award);
        if (award.coins != 0)
        {
            awardText.text = "coins: " + award.coins;

        }
        else if (award.ships != 0)
        {
            awardText.text = "ships: " + award.ships;

        }
        if (award.bonus != 0)
        {
            awardText.text = "bonus: " + award.bonus;

        }
    }

    public void ChooseBadge()
    {
        Objects.instance.badgePanel.transform.GetChild(6).GetComponent<AchievementCompleteButton>().achievementID =
            achievement.id;
        Objects.instance.badgePanel.transform.parent.gameObject.SetActive(true);
        Objects.instance.badgePanel.transform.GetChild(2).GetComponent<Image>().sprite = 
            transform.GetChild(0).GetComponent<Image>().sprite;
        Objects.instance.badgePanel.transform.GetChild(3).GetComponent<TMP_Text>().text = 
            gameObject.GetComponent<BadgeEntry>().title.text;
        Objects.instance.badgePanel.transform.GetChild(4).GetComponent<TMP_Text>().text = 
            gameObject.GetComponent<BadgeEntry>().description.text;
        
        
        
        StartCoroutine(SetBadgeProgression(Objects.instance.badgePanel.transform.GetChild(5).gameObject));
    }

    private IEnumerator SetBadgeProgression(GameObject progressSlider)
    {
        if (progressSlider == null)
        {
            Debug.LogError("progressSlider == null");
            yield break;
        }


        int condition = JSONTest.UserLocalDataObject.AchievementControlList[achievement.id - 1].condition
            .ConditionReturnInt();
        float curent = 0;

        if (JSONTest.UserLocalDataObject.AchievementControlList[achievement.id - 1]
                .AchievementControllerInitializer() != null)
        {
            curent = JSONTest.UserLocalDataObject.AchievementControlList[achievement.id - 1]
                .AchievementControllerInitializer().CheckAndReturnInt(achievement.id - 1);
        }
        else
            curent = -1;
        
        if (condition != -1)
        {
            progressSlider.GetComponent<Slider>().value = curent / condition;
            if (progressSlider.transform.GetChild(1) == null)
            {
                Debug.LogError("progressSlider.transform.GetChild(1) == null");
                yield break;
            }

            if (progressSlider.transform.GetChild(1).GetChild(1) == null)
            {
                Debug.LogError("progressSlider.transform.GetChild(1).GetChild(1) == null");
                yield break;
            }

            if (curent > condition)
            {
                progressSlider.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text =
                    condition + " / " + condition;
            }
            else
                progressSlider.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text =
                    curent + " / " + condition;

        }
        else
        {
            progressSlider.GetComponent<Slider>().value = 0;
            progressSlider.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = "";
        }

        yield return null;
    }
}
