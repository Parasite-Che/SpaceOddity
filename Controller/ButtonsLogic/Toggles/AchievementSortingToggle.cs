using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSortingToggle : MonoBehaviour, IToggleController
{
    
    
    public void OnToggleOn()
    {
        BadgesActivatingSorting(Objects.instance.entryGameplayBadgesContainer_);
        BadgesActivatingSorting(Objects.instance.entrySocialBadgesBContainer_);
    }

    public void OnToggleOff()
    {
        ActivateAllBadges(Objects.instance.entryGameplayBadgesContainer_);
        ActivateAllBadges(Objects.instance.entrySocialBadgesBContainer_);
    }
    
    public void BadgesActivatingSorting(Transform content)
    {
        foreach (Transform badge in content)
        {
            if (JSONTest.UserLocalDataObject.AchievementControlList[badge.GetComponent<BadgeEntry>().achievement.id - 1].finished == false)
            {
                badge.gameObject.SetActive(false);
            }
        }
    }


    private void ActivateAllBadges(Transform content)
    {
        foreach (Transform badge in content)
        {
            badge.gameObject.SetActive(true);
        }    
    }
}
