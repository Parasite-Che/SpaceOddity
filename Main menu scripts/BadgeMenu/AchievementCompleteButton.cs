using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementCompleteButton : MonoBehaviour
{
    public int achievementID = -1;
    public Texture2D[] icons;
    
    private void OnEnable()
    {
        AchievementCheck();
        
        if (JSONTest.UserLocalDataObject.AchievementControlList[achievementID - 1].finished &&
            JSONTest.UserLocalDataObject.AchievementControlList[achievementID - 1].awardClaimed == false)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                JSONTest.UserLocalDataObject.GetAward(achievementID - 1);
                AchievementAwardClaimed();
                Objects.instance.badgePanel.transform.GetChild(6).GetComponent<Button>().interactable = false;
                JSONTest.SaveUserLocalDataOnDevice();
                CheckAchievementsAndDeleteNotice();
            });

            AchievementComplete();
            Objects.instance.badgePanel.transform.GetChild(6).GetComponent<Button>().interactable = true;

        }
        else
        {
            AchievementUnComplete();
            Objects.instance.badgePanel.transform.GetChild(6).GetComponent<Button>().interactable = false;
        }
    }

    private void OnDisable()
    {
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    private void CheckAchievementsAndDeleteNotice()
    {
        foreach (var achievement in JSONTest.UserLocalDataObject.AchievementControlList)
        {
            if (achievement.finished == true && achievement.awardClaimed == false)
            { 
                Objects.instance.BadgesNotice.SetActive(true);
                return;
            }
        }
        Objects.instance.BadgesNotice.SetActive(false);
    }

    private void AchievementCheck()
    {
        int intAward;

        if (JSONTest.UserLocalDataObject == null)
        {
            return;
        }

        if (JSONTest.UserLocalDataObject.AchievementControlList[achievementID - 1].awardClaimed == false)
        {
            //Debug.LogError("достижение: " + achievementID);
            intAward = JSONTest.UserLocalDataObject.AchievementControlList[achievementID - 1].award.AwardReturn();
            //Debug.LogError(intAward);
            if (intAward != -1)
            {
                IconChanging();
                gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = intAward.ToString();
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
            }

        }
        else
        {
            AchievementAwardClaimed();
        }
    }

    public void IconChanging()
    {
        if (JSONTest.UserLocalDataObject.AchievementControlList[achievementID - 1].award.AwardTypeReturn() == "coins")
        {
            gameObject.transform.GetChild(1).GetComponent<Image>().sprite = TextureToSprite(icons[0]);
        }
        else if (JSONTest.UserLocalDataObject.AchievementControlList[achievementID - 1].award.AwardTypeReturn() == "ships")
        {
            gameObject.transform.GetChild(1).GetComponent<Image>().sprite = TextureToSprite(icons[1]);
        }
        else if (JSONTest.UserLocalDataObject.AchievementControlList[achievementID - 1].award.AwardTypeReturn() == "bonus")
        {
            gameObject.transform.GetChild(1).GetComponent<Image>().sprite = TextureToSprite(icons[2]);
        }
        else
        {
            Debug.LogError("Something with icon");
        }
    }

    private Sprite TextureToSprite(Texture2D tex)
    {
        Rect rec = new Rect(0, 0, tex.width, tex.height);
        return Sprite.Create(tex, rec, new Vector2(0, 0), 1);
    }
    
    private void AchievementAwardClaimed()
    {
        gameObject.GetComponent<Image>().color = new Color(0.314f, 0.2641509f, 0.539f, 0.7411765f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        //gameObject.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1,1,1,0.6f);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        //gameObject.transform.GetChild(1).GetComponent<Image>().color = new Color(0.8584906f,0.8584906f,0.8584906f,0.6392157f);
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
        gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = "Claimed!";
        gameObject.transform.GetChild(2).GetComponent<TMP_Text>().color = new Color(1,1,1,0.6f);
    }

    private void AchievementComplete()
    {
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1,1,1,1);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,1);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).GetComponent<TMP_Text>().color = new Color(1,1,1,1);

    }
    
    private void AchievementUnComplete()
    {
        gameObject.GetComponent<Image>().color = new Color(0.314f, 0.2641509f, 0.539f, 0.7411765f);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1,1,1,0.6f);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).GetComponent<Image>().color = new Color(0.8584906f,0.8584906f,0.8584906f,0.6392157f);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).GetComponent<TMP_Text>().color = new Color(1,1,1,0.6f);

    }

}
