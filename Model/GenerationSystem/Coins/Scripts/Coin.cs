using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Coin : MonoBehaviour, IPlayerCollision, IPlayerAttraction, IDeletable
{
    public int reward;
    public bool CanAttract { get; set; }
    public Transform coinTransform;

    private Coroutine _cor;

    public Transform AttractionTarget { get; set; }

    private void Start()
    {
        Events.onBonusActivated += StartAttracting;
        Events.onBonusDeactivated += StopAttracting;
    }

    public void InitializeCoin(CoinScriptableObject source)
    {
        coinTransform = gameObject.transform;
        reward = source.reward;
        gameObject.GetComponent<SpriteRenderer>().sprite= source.sprite;
    }

    #region Collision
    public void ActOnTriggerEnter(GameObject player)
    {
        Debug.Log("Picked up a coin");
        Events.onCoinPickedUp?.Invoke(this);
        GetDeleted();
    }

    public void ActOnTriggerExit(GameObject player)
    {
        ;
    }
    #endregion

    #region Attraction
    public void EnterPlayerAttractionRange(GameObject player)
    {
        CanAttract = true;
        AttractionTarget = player.transform;

        if (BonusManager.instance.BonusIsActive(Bonuses.type.magnet))
        {
            StartAttracting(Bonuses.type.magnet);
        }
    }

    public void ExitPlayerAttractionRange(GameObject player)
    {
        CanAttract = false;
        AttractionTarget = null;

        StopAttracting(Bonuses.type.magnet);
    }

    public void StartAttracting(Bonuses.type bonusType)
    {
        if (CanAttract && bonusType == Bonuses.type.magnet)
        {
            _cor = StartCoroutine(Attract(AttractionTarget));
        }
    }

    public void StopAttracting(Bonuses.type type)
    {
        if (_cor != null) StopCoroutine(_cor);
        _cor = null;
    }

    public IEnumerator Attract(Transform player)
    {
        while (CanAttract)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 50 * Time.deltaTime);
            yield return null;
        }
    }
    #endregion

    public void GetDeleted()
    {
        gameObject.SetActive(false);
        CoinsGenerator.Instance.activeCoinList.Remove(this);
    }
}
