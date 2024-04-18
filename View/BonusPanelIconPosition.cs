using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusPanelIconPosition : MonoBehaviour
{
    public RectTransform rectTransform;
    public Bonus bonus;

    [SerializeField] private Sprite fillSprite;
    [SerializeField] private Image bonusSprite;
    [SerializeField] private Image fill;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        fill.fillAmount = bonus.fraction;
    }

    public void ActivateBonusSlot(Bonus bonus)
    {
        this.bonus = bonus;
        bonusSprite.sprite = bonus.bonus.sprite;
        fill.fillAmount = bonus.fraction;
        gameObject.SetActive(true);
    }

    public void DeactivateBonusSlot()
    {
        gameObject.SetActive(false);
        this.bonus = null;
        bonusSprite.sprite = null;
        fill.fillAmount = 1f;
    }
}
