using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BonusPool : MonoBehaviour
{
    public ObjectPool<BonusCollision> pool;
    public int defaultCapacity;
    public int maxCapacity;
    public int activeObjectsInPool;
    public int inactiveObjectsInPool;

    [SerializeField] private Transform bonusParent;

    public void Initialize()
    {
        pool = new ObjectPool<BonusCollision>(CreateBonus, GetBonusFromPool, ReturnBonusToPool, DestroyBonus, true, defaultCapacity, maxCapacity);
    }

    private void Update()
    {
        activeObjectsInPool = pool.CountActive;
        inactiveObjectsInPool = pool.CountInactive;
    }

    #region BonusPool
    public BonusCollision CreateBonus()
    {
        BonusCollision bonus = Instantiate(Objects.instance.bonusPrefab, bonusParent).GetComponent<BonusCollision>();
        bonus.transform.rotation = Quaternion.identity;
        bonus.SetPool(pool);
        bonus.isReleased = false;
        return bonus;
    }

    public void GetBonusFromPool(BonusCollision bonus)
    {
        bonus.gameObject.SetActive(true);
        bonus.isReleased = false;
    }

    public void ReturnBonusToPool(BonusCollision bonus)
    {
        bonus.isReleased = true;
        bonus.gameObject.SetActive(false);
    }

    public void DestroyBonus(BonusCollision bonus)
    {
        bonus.isReleased = true;
        Destroy(bonus.gameObject);
    }
    #endregion
}
