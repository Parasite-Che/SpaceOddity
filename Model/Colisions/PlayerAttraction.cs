using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPlayerAttraction IPA))
        {
            IPA.EnterPlayerAttractionRange(transform.parent.gameObject);
            //Debug.Log("YES");
        }
        else
        {
            //Debug.Log("NO");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPlayerAttraction IPA))
        {
            IPA.ExitPlayerAttractionRange(transform.parent.gameObject);
            //Debug.Log("YES");
        }
        else
        {
            //Debug.Log("NO");
        }
    }
}

interface IPlayerAttraction
{
    bool CanAttract { get; set; }
    Transform AttractionTarget { get; set; }

    public void EnterPlayerAttractionRange(GameObject player);

    public void ExitPlayerAttractionRange(GameObject player);

    public void StartAttracting(Bonuses.type bonusType);

    public void StopAttracting(Bonuses.type bonusType);

    public IEnumerator Attract(Transform player);
}
