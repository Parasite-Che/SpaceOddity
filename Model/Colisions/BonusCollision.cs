using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BonusCollision : MonoBehaviour, IPlayerCollision, IPlayerAttraction, IDeletable
{
    public BonusScriptableObject bonus;

    private Coroutine _cor;
    public bool CanAttract { get; set; }
    public Transform AttractionTarget { get; set; }

    private ObjectPool<BonusCollision> pool;
    public bool isReleased;

    [SerializeField] private Vector3 position;



    public void LoadVisualBonusData(BonusScriptableObject type)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = type.sprite;
        gameObject.name = type.name;
        bonus = type;
    }

    private void Start()
    {
        Events.onBonusActivated += StartAttracting;
        Events.onBonusDeactivated += StopAttracting;
    }

    #region IPlayerCollision
    public void ActOnTriggerEnter(GameObject player)
    {
        Debug.Log("Pick up bonus");
        Events.onBonusPickUp?.Invoke(bonus);
        GetDeleted();
    }

    public void ActOnTriggerExit(GameObject player)
    {
        ;
    }
    #endregion

    #region IPlayerAttraction
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
        if (isReleased) return;
        pool.Release(this);
        BonusManager.instance.bonusesOnScene.Remove(this);
    }

    public void SetPool(ObjectPool<BonusCollision> pool)
    {
        this.pool = pool;
    }
}
