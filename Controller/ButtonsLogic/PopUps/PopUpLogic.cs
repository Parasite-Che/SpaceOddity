using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpLogic : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 zeroPos;

    private void Awake()
    {
        startPos = gameObject.transform.localPosition;
        zeroPos = new Vector3(0, 0, -2);
    }
    
    public IEnumerator LoadAsync()
    {
        if (gameObject.transform.localPosition.x == 0 && gameObject.transform.localPosition.y == 0)
        {
            float lerp = 0;
            while (gameObject.transform.localPosition != startPos)
            {
                if (lerp > 1)
                {
                    lerp = 1;
                }
                gameObject.transform.localPosition = Vector3.Lerp(zeroPos, startPos, lerp);
                lerp += 0.0392157f;
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("I'm Back!");
        }
        else
        {
            float lerp = 0;
            while (gameObject.transform.localPosition != zeroPos)
            {
                if (lerp > 1)
                {
                    lerp = 1;
                }
                gameObject.transform.localPosition = Vector3.Lerp(startPos, zeroPos, lerp);
                lerp += 0.0392157f;
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("I'm come!");
        }
    }

    public IEnumerator SmoothActivationObj(GameObject obj, Color32 start, Color32 end, int steps)
    {
        for (int i = 0; i <= steps; i++)
        {
            var lerpingProcess = (float)i / steps;
            obj.GetComponent<Image>().color = new Color32(start.r, start.g, start.b, (byte)Mathf.Lerp(start.a, end.a, lerpingProcess));
            //Debug.Log(Mathf.Lerp(start.a, end.a, lerpingProcess));
            yield return null;
        }
        //Debug.Log(obj.GetComponent<Image>().color);
    }

    public IEnumerator DeactivatePanel(GameObject panel)
    {
        while (panel.GetComponent<Image>().color.a != 0)
        {
            yield return null;
        }
        panel.SetActive(false);
    }
}
