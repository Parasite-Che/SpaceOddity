using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class BonusStatusView : MonoBehaviour
{
    public List<Bonus> bonuses;
    [SerializeField] private List<BonusPanelIconPosition> bonusIconPositions;

    public void ActivateBonus(Bonus bonus)
    {
        bonuses.Add(bonus);
        for (int i = 0; i < bonusIconPositions.Count; i++)
        {
            if (bonusIconPositions[i].bonus == null)
            {
                bonusIconPositions[i].ActivateBonusSlot(bonus);
                return;
            }
        }
    }

    public void DeactivateBonus(Bonus bonus)
    {
        for (int i = 0; i < bonusIconPositions.Count; i++)
        {
            if (bonusIconPositions[i].bonus == bonus)
            {
                bonusIconPositions[i].DeactivateBonusSlot();
            }
        }
        bonuses.Remove(bonus);
        ReassignBonusIconPositions();
    }
    
    public void ReassignBonusIconPositions()
    {
        for (int i = 1; i < bonusIconPositions.Count; i++)
        {
            if (bonusIconPositions[i - 1].bonus == null && bonusIconPositions[i].bonus != null)
            {
                Bonus tmp = bonusIconPositions[i].bonus;
                bonusIconPositions[i].DeactivateBonusSlot();
                bonusIconPositions[i - 1].ActivateBonusSlot(tmp);
            }
        }
    }

}
